using Product.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductContext context;

        public UnitOfWork(ProductContext context)
        {
            this.context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
           return await context.SaveChangesAsync();
        }
    }
}
