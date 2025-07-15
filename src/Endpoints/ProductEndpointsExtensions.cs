using ContainerizedAPI.DTOs;
using ContainerizedAPI.Services;

public static class ProductEndpointsExtensions
{
    public static void MapProductEndpoints(this WebApplication app, IProductService productService)
    {
        var productsGroup = app.MapGroup("product").WithTags("Products");
        productsGroup.MapPost("/", async (CreateProductRequest productDTO) =>
        {
            var productRequest = new ProductRequest
            {
                Name = productDTO.Name,
                Sku = productDTO.Sku
            };
            await productService.UpsertProduct(productRequest);
            // Cache the product after creation

            return Results.Created();
        })
        .WithSummary("Create new product")
        .WithDescription("Creates a new product with provided data");

        productsGroup.MapGet("/", async () =>
        {
            var products = await productService.GetProducts();
            return Results.Ok(products);
        })
        .WithSummary("Get all products")
        .WithDescription("Retrieves a list of all available products.");

        productsGroup.MapGet("/{id:guid}", async (Guid id) =>
        {
            var product = await productService.GetProductById(id);

            if (product is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(product);
        })
        .WithSummary("Get a product by id")
        .WithDescription("Retrieves a product by id if available.");


        productsGroup.MapPatch("/UpdateSku", async (UpdateSkuRequest req) =>
        {
            var product = await productService.UpdateSku(req);
            return product is not null
                ? Results.Ok(product)
                : Results.NotFound();
        })
        .WithSummary("Updates a product SKU")
        .WithDescription("Updates product with provided SKU");


        productsGroup.MapPut("/{id:guid}", async (Guid id, ProductRequest req) =>
        {
            var product = await productService.UpsertProduct(req);
            return product is not null
                ? Results.Ok(product)
                : Results.NotFound();
        })
       .WithSummary("Updates a product SKU")
       .WithDescription("Updates product with provided SKU");

        productsGroup.MapDelete("/{id:guid}", async (Guid id) =>
        {
            var isExisting = await productService.DeleteProduct(id);
            return isExisting
                ? Results.NoContent()
                : Results.NotFound();
        })
        .WithSummary("Deletes a product by id")
        .WithDescription("Deletes the provided product");


    }
}
