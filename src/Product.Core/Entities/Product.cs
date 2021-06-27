using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal NormalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public string ImageUrl { get; set; }


        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
