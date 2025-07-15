using ContainerizedAPI.Entities;

namespace ContainerizedAPI.DTOs;

// Add Id to DTO for API responses
public class ProductRequest
{
    public string? Name { get; set; }
    public int Sku { get; set; }
    public Guid Id { get; set; }
    public ProductRequest()
    {

    }
    public ProductRequest(string name, int sku)
    {
        Name = name;
        Sku = sku;
    }



}

public static class ProductExtensions
{
    public static Product MapToProduct(this ProductRequest productDTO)
    {
        return new Product
        {
            Name = productDTO.Name,
            Sku = productDTO.Sku,
            Id = productDTO.Id,
        };
    }
}


