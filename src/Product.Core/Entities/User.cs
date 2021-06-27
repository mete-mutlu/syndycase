using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
