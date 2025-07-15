using ContainerizedAPI;
using ContainerizedAPI.Services;
using ContainerizedAPI.src;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add redis cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IProductService, ProductService>();
// Ensure services are built before using them in the app
var productService = builder.Services.BuildServiceProvider().GetRequiredService<IProductService>();

var app = builder.Build();
app.MapGet("/", () => "Hello from docker").WithTags("Ping");


// Move product endpoints to extension method
app.MapProductEndpoints(productService);

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



