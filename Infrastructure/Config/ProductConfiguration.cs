using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    //interfejsom se mora implementirati sve što on zahtijeva
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
    {
        //ovdje se može specificirati za svaki tip podataka iz bilo koje klase 
        //da ne bismo dobijali warning prilikom migracije za precision podatka Price
        builder.Property(X => X.Price).HasColumnType("decimal(18,2)");
    }
}
