using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTB.Eshop.Domain.Entities
{
    public class OrderItem : Entity<int>
    {
        [ForeignKey(nameof(Order))]
        public int OrderID { get; set; }

        [ForeignKey(nameof(Zvire))]
        public int ZvireID { get; set; }

        [Required]
        public int Amount { get; set; }
        [Required]
        public double Price { get; set; }

        public Order? Order { get; set; }
        public Zvire? Zvire { get; set; }
    }
}
