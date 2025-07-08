using ContainerizedAPI.Entities;

namespace ContainerizedAPI.DTOs;

// Add Id to DTO for API responses
public record ProductDTO(Guid Id, string Name, int Sku)
{
    public ProductDTO(string Name, int Sku) : this(Guid.Empty, Name, Sku) { }
}

public static class ProductExtensions
{
    public static Product MapToProduct(this ProductDTO productDTO)
    {
        return new Product
        {
            Name = productDTO.Name,
            Sku = productDTO.Sku,
        };
    }
}
