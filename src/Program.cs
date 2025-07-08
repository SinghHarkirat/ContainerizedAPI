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

productsGroup.MapGet("/", async (MyDbContext dbContext) =>
    Results.Ok(await dbContext.Products
        .Select(p => new ProductDTO(p.Id, p.Name, p.Sku))
        .ToListAsync()))
    .WithSummary("Get all products")
    .WithDescription("Retrieves a list of all available products.");

productsGroup.MapGet("/{id:guid}", async (Guid id, MyDbContext dbContext) =>
{
    var product = await dbContext.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new ProductDTO(product.Id, product.Name, product.Sku));
})
.WithSummary("Get a product by id")
.WithDescription("Retrieves a product by id if available.");

// PATCH: expects JSON body { "id": "...", "newSku": ... }
productsGroup.MapPatch("/UpdateSku", async (UpdateSkuRequest req, MyDbContext dbContext) =>
{
    var product = await dbContext.Products.FindAsync(req.Id);
    if (product is null)
    {
        return Results.NotFound();
    }
    product.Sku = req.NewSku;
    product.UpdatedDate = DateTime.Now;
    await dbContext.SaveChangesAsync();
    return Results.Ok(new ProductDTO(product.Id, product.Name, product.Sku));
})
.WithSummary("Updates a product SKU")
.WithDescription("Updates product with provided SKU");


productsGroup.MapDelete("/{id:guid}", async (Guid id, MyDbContext dbContext) =>
{
    var product = await dbContext.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound();
    }
    dbContext.Products.Remove(product);
    await dbContext.SaveChangesAsync();
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

public record UpdateSkuRequest(Guid Id, int NewSku);
