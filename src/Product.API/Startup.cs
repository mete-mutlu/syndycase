using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Product.Core.Abstract;
using Product.Infrastructure.EntityFramework;
using Product.Infrastructure.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc().AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product.API", Version = "v1" });
            });
            services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("ProductDb"));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ProductContext>();
                var brands = new List<Core.Entities.Brand>
            {
                new Core.Entities.Brand {Id=1, Name = "Brand A" },
                new Core.Entities.Brand {Id=2, Name = "Brand B" },
                new Core.Entities.Brand {Id=3, Name = "Brand C" }
            };
                context.AddRange(brands);


                var users = new List<Core.Entities.User>
            {
                new Core.Entities.User {Id=1, Username = "User A" },
                new Core.Entities.User {Id=2, Username = "User B" },
                new Core.Entities.User {Id=3, Username = "User C" }
            };
                context.AddRange(users);


                var products = new List<Core.Entities.Product>
            {
                new Core.Entities.Product {Id=1, Name = "Product A",Description="Desc", NormalPrice=10, DiscountedPrice= 7.5M,ImageUrl="url",UserId =1, BrandId=1 },
                new Core.Entities.Product {Id=2, Name = "Product B",Description="Desc", NormalPrice=4, DiscountedPrice= 2,ImageUrl="url",UserId =2, BrandId=2 },
                new Core.Entities.Product {Id=3, Name = "Product C",Description="Desc", NormalPrice=100, DiscountedPrice= 88,ImageUrl="url",UserId =3, BrandId=3 },
                new Core.Entities.Product {Id=4, Name = "Product D",Description="Desc", NormalPrice=23, DiscountedPrice= 14,ImageUrl="url",UserId =3, BrandId=3 },
            };
                context.AddRange(products);

                context.SaveChanges();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
