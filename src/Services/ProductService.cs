using ContainerizedAPI.DTOs;
using ContainerizedAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ContainerizedAPI.Services;

public class ProductService(MyDbContext myDbContext, IDistributedCache cache) : IProductService
{
    public async Task<List<ProductRequest>> GetProducts()
    {
        List<ProductRequest> products = new List<ProductRequest>();
        // Check if products are cached
        var productsJson = await cache.GetStringAsync("products");
        if (productsJson != null)
        {
            products = JsonConvert.DeserializeObject<List<ProductRequest>>(productsJson)!;

        }
        else
        {
            products = await myDbContext.Products
          .Select(p => new ProductRequest()
          {
              Id = p.Id,
              Name = p.Name,
              Sku = p.Sku
          })
          .ToListAsync();
            // Cache the products
            await cache.SetStringAsync("products", JsonConvert.SerializeObject(products), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache for 5 minutes
            });
        }
        return products;
    }
    public async Task<Product> GetProductById(Guid id)
    {
        var product = await myDbContext.Products.FindAsync(id);
        return product;
    }
    public async Task<ProductRequest> UpsertProduct(ProductRequest productRequest)
    {
        Product product = await GetProductById(productRequest.Id);
        if (product is null)
        {
            product = new Product
            {
                Name = productRequest.Name,
                Sku = productRequest.Sku,
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
            };
            myDbContext.Products.Add(product);
            await myDbContext.SaveChangesAsync();
            await cache.RemoveAsync("products");
        }
        else
        {
            product.Name = productRequest.Name;
            product.Sku = productRequest.Sku;
            product.UpdatedDate = DateTime.Now;
            var updateTask = myDbContext.SaveChangesAsync();
            // Update the cache
            var deleteCacheTask = cache.RemoveAsync("products");
            await Task.WhenAll(updateTask, deleteCacheTask);
        }
        return new ProductRequest
        {
            Id = product.Id,
            Name = product.Name,
            Sku = product.Sku
        };
    }
    public async Task<ProductRequest> UpdateSku(UpdateSkuRequest productRequest)
    {

        var product = await GetProductById(productRequest.Id);
        product.Sku = productRequest.NewSku;
        product.UpdatedDate = DateTime.Now;
        myDbContext.Products.Update(product);
        var updateTask = myDbContext.SaveChangesAsync();
        // Update the cache
        var deleteCacheTask = cache.RemoveAsync("products");
        await Task.WhenAll(updateTask, deleteCacheTask);
        return new ProductRequest
        {
            Id = product.Id,
            Name = product.Name,
            Sku = product.Sku
        };
    }
    public async Task<bool> DeleteProduct(Guid id)
    {
        bool isExisting = true;
        var product = await myDbContext.Products.FindAsync(id);
        if (product is null)
        {
            isExisting = false;
        }
        myDbContext.Products.Remove(product);
        await myDbContext.SaveChangesAsync();
        // Update the cache
        await cache.RemoveAsync("products");
        return isExisting;
    }
}
