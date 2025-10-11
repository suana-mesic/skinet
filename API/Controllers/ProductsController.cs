using System;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
//ruta će se zvati ProductsController-Controller=api/products
//svaki put kada se treba instancirati iProductRepository (interfejs)
//instancirat će se zapravo klasa ProductRepository (tako smo napisali servis u Program.cs)
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    //izbrisat ćemo stari ctor i umjesto da u ovom kontroleru koristimo StoreContext
    //koristit ćemo ProductRepository (jer on implementira sve ove metode kjoje su bile u ovom kontroleru)
    //ovo je http endpoint
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        var spec = new ProducSpecification(brand, type, sort);

        var products = await repo.ListAsync(spec);

        return Ok(products);
    }

    [HttpGet("{id:int}")] //api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null)
            return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);

        if (await repo.SaveAllAsync())
        {
            //rezultat ove metode je 
            //Status 201
            // Lokaciju novog resursa u headeru Location, npr. /api/products/2
            // I sam objekat product u body-u odgovora
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product!");

        //product recimo ima izmijeneno polje "Name"
        //Entry funkcija zapravo uzima izmijenjen proizvod i onda samo preko 
        //starog proizvoda prelijepi informacije koje su u novom 
        //(uključujući i one koje nisu promijenjene)
        repo.Update(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        repo.Remove(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await repo.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await repo.ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return repo.Exists(id);
    }
}
