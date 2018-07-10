using System;
using System.Collections.Generic;

namespace Project1.Context.Models
{
    public partial class Locations
    {
        public Locations()
        {
            Orders = new HashSet<Orders>();
        }

        public int LocationId { get; set; }
        public string Address { get; set; }
        public int ToppingInventoryPepperoni { get; set; }
        public int ToppingInventoryCheese { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
