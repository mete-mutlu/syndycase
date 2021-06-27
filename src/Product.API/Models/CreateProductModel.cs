using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Models
{
    public class CreateProductModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal NormalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public string ImageUrl { get; set; }

        public int BrandId { get; set; }

        public int UserId { get; set; }
    }
}
