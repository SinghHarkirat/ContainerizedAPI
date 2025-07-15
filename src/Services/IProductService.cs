using ContainerizedAPI.DTOs;
using ContainerizedAPI.Entities;

namespace ContainerizedAPI.Services;

public interface IProductService
{
    Task<List<ProductRequest>> GetProducts();
    Task<Product> GetProductById(Guid id);
    Task<ProductRequest> UpsertProduct(ProductRequest productRequest);
    Task<ProductRequest> UpdateSku(UpdateSkuRequest productRequest);
    Task<bool> DeleteProduct(Guid id);
}
