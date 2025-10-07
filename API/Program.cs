using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//servis koji je AddScoped - scoped to the request level - živi onoliko dugo koliko traje http request
//AddSingleton - kreira servis kada aplikacija starta, i gasi se tek kada se aplikacija ugasi
//AddTransient - scoped to the method level
builder.Services.AddScoped<iProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.MapControllers();

// Kod  Uloga u fabrici čokolade (GPT help)
// CreateScope()    Otvaranje jedne proizvodne smjene
// scope.ServiceProvider    Pristup skladištu s registriranim mašinama
// GetRequiredService<StoreContext>()   Uzimaš konkretno: mašinu za pakovanje čokolade (bazu podataka)
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (System.Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();

