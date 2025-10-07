using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    //ovo je static metoda tako da možemo koristiti metodu SeedAsync bez da 
    //kreiramo instancu klase StoreContextSeed
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            if (products == null)
                return;
            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}
