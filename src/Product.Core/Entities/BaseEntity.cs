using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Product.Core.Enums;

namespace Product.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }
}
