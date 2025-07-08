using ContainerizedAPI;
using ContainerizedAPI.DTOs;
using ContainerizedAPI.src;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();
app.MapGet("/", () => "Hello from docker").WithTags("Ping");

var productsGroup = app.MapGroup("product").WithTags("Products");
productsGroup.MapPost("/", async (ProductDTO productDTO, MyDbContext myDbContext) =>
{
    myDbContext.Products.Add(productDTO.MapToProduct());
    await myDbContext.SaveChangesAsync();
    return Results.Created();
})
.WithSummary("Create new product")
.WithDescription("Creates a new product with provided data");

productsGroup.MapGet("/", async (MyDbContext dbContext) => Results.Ok(await dbContext.Products.ToListAsync()))
            .WithSummary("Get all products")
            .WithDescription("Retrieves a list of all available products.");

productsGroup.MapGet("/{id:guid}", async (Guid id, MyDbContext myDbContext) =>
{
    var product = await myDbContext.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new ProductDTO(product.Name, product.Sku));
})
.WithSummary("Get a product by id")
.WithDescription("Retrieves a product by id if available.");

productsGroup.MapPatch("/UpdateSku", async (Guid id, int newSku, MyDbContext dbContext) =>
{
    var product = await dbContext.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound();
    }
    product.Sku = newSku;
    product.UpdatedDate = DateTime.Now;
    await dbContext.SaveChangesAsync();
    return Results.Ok(product);
})
.WithSummary("Updates a product SKU")
.WithDescription("Updates product with provided SKU");


productsGroup.MapDelete("/{id:Guid}", async (MyDbContext myDbContext, Guid id) =>
{
    var product = await myDbContext.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound();
    }
    myDbContext.Products.Remove(product);
    await myDbContext.SaveChangesAsync();
    return Results.Ok();
})
.WithSummary("Deletes a product by id")
.WithDescription("Deletes the provided product");
// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
