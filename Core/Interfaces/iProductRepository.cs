using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    //Metode sa Get su Asinhrone zato što pozivaju bazu da dadne podatke
    //Ostale metode nisu jer delete, insert i modify se primjenjuju kada se 
    //applajaju promjene nad bazom podataka

    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);
    //ne mora vratiti proizvod (jer moze da ne postoji)
    Task<Product?> GetProductByIdAsync(int id);
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<IReadOnlyList<string>> GetTypesAsync();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool ProductExists(int id);
    Task<bool> SaveChangesAsync();
}
