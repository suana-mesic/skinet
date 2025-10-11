using System;
using Core.Entities;

namespace Core.Specifications;

public class ProducSpecification : BaseSpecification<Product>
{
    //poziva bazni konstruktor klase BaseSpecification i prosljeđuje mu Expression<Fun<Product,bool>>
    //Product -> 1. parametar je zapravno ono s čim se pravi taj expression
    //bool -> 2. parametar je ono što Func vraća
    public ProducSpecification(string? brand, string? type, string? sort) : base(x =>
        (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) &&
        (string.IsNullOrWhiteSpace(type) || x.Type == type)
    )
    {
        switch (sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }

}
