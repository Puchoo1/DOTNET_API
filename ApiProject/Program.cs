using Microsoft.EntityFrameworkCore;
using ApiProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MySQL Database Connection
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//     new MySqlServerVersion(new Version(8, 3, 0))));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        "Server=sql12.freesqldatabase.com;Database=sql12761971;User=sql12761971;Password=aDKYiSsPHp;SslMode=Preferred;",
        new MySqlServerVersion(new Version(8, 0, 0))
    ));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Creating the endpoints
// ✅ GET all products
app.MapGet("/api/products", async (ApplicationDbContext db) => 
    await db.Products.ToListAsync()
);

// ✅ GET product by ID
app.MapGet("/api/products/{id}", async (int id, ApplicationDbContext db) =>
    await db.Products.FindAsync(id) is Product product 
        ? Results.Ok(product) 
        : Results.NotFound()
);

// ✅ CREATE new product
app.MapPost("/api/products", async (Product product, ApplicationDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{product.Id}", product);
});

// ✅ UPDATE product by ID
app.MapPut("/api/products/{id}", async (int id, Product updatedProduct, ApplicationDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product == null) return Results.NotFound();

    product.Name = updatedProduct.Name;
    product.Price = updatedProduct.Price;
    product.Stock = updatedProduct.Stock;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ✅ DELETE product by ID
app.MapDelete("/api/products/{id}", async (int id, ApplicationDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product == null) return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


var port = Environment.GetEnvironmentVariable("PORT") ?? "5236"; // Default to 5236 if not set
app.Run($"http://0.0.0.0:{port}");

app.Run(); 


