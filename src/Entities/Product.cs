namespace ContainerizedAPI.Entities;

public class Product : BaseEntity
{
    public Product()
    {
        CreatedBy = "admin";
    }
    public string Name { get; set; }
    public int Sku { get; set; }
}
