using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Product.Core;
using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities = Product.Core.Entities;

namespace Product.Infrastructure.EntityFramework
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Entities.Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
             .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .HasQueryFilter(ConvertFilterExpression<BaseEntity>(e => e.Status != Enums.EntityStatus.Archived, entityType.ClrType));
            }

        }

        private static LambdaExpression ConvertFilterExpression<TInterface>(
         Expression<Func<TInterface, bool>> filterExpression,
         Type entityType)
        {
            var newParam = Expression.Parameter(entityType);
            var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

            return Expression.Lambda(newBody, newParam);
        }
    }
}
