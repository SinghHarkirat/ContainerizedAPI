using ContainerizedAPI.Entities;

namespace ContainerizedAPI.DTOs;

public record ProductDTO(string Name, int Sku);

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
