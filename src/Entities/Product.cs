namespace ContainerizedAPI.Entities;

public class Product : BaseEntity
{
    public Product()
    {
        CreatedDate = DateTime.Now;
        UpdatedDate = DateTime.Now;
        CreatedBy = "admin";
    }
    public string Name { get; set; }
    public int Sku { get; set; }
}
