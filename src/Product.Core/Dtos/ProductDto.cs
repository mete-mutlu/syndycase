using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal NormalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public string ImageUrl { get; set; }

        public string Username { get; set; }

        public string BrandName { get; set; }
    }
}
