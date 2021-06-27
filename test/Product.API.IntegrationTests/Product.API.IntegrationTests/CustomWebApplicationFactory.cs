using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Product.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Entities = Product.Core.Entities;
using AutoFixture;

namespace Product.API.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ProductContext>));

                services.Remove(descriptor);

                services.AddDbContext<ProductContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ProductContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        private void InitializeDbForTests(ProductContext context)
        {
            var brands = new List<Entities.Brand>
            {
                new Entities.Brand {Id=1, Name = "Brand1" },
                new Entities.Brand {Id=2, Name = "Brand2" },
                new Entities.Brand {Id=3, Name = "Brand3" }
            };
            context.AddRange(brands);


            var users = new List<Entities.User>
            {
                new Entities.User {Id=1, Username = "User1" },
                new Entities.User {Id=2, Username = "User2" },
                new Entities.User {Id=3, Username = "User3" }
            };
            context.AddRange(users);


            var products = new List<Entities.Product>
            {
                new Entities.Product {Id=1, Name = "Product1",Description="Desc", NormalPrice=10, DiscountedPrice= 7.5M,ImageUrl="url",UserId =1, BrandId=1 },
                new Entities.Product {Id=2, Name = "Product2",Description="Desc", NormalPrice=4, DiscountedPrice= 2,ImageUrl="url",UserId =2, BrandId=2 },
                new Entities.Product {Id=3, Name = "Product3",Description="Desc", NormalPrice=100, DiscountedPrice= 88,ImageUrl="url",UserId =3, BrandId=3 },
                new Entities.Product {Id=4, Name = "Product4",Description="Desc", NormalPrice=23, DiscountedPrice= 14,ImageUrl="url",UserId =3, BrandId=3 },
            };
            context.AddRange(products);


            context.SaveChanges();
        }
    }
}