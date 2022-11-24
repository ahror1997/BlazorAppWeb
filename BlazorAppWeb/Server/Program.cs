global using BlazorAppWeb.Shared.Models;
using BlazorAppWeb.Server.Data;
using BlazorAppWeb.Server.Services.CategoryService;
using BlazorAppWeb.Server.Services.ProductService;
using BlazorAppWeb.Server.Services.CartService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DB Connection
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
{
    // mssql
    //options.UseSqlServer(connectionString);

    // mysql
    //options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

    // postgresql
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();


var app = builder.Build();

app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
