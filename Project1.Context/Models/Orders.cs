using System;
using System.Collections.Generic;

namespace Project1.Context.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderPizza = new HashSet<OrderPizza>();
        }

        public int OrderId { get; set; }
        public int LocationId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal TotalPrice { get; set; }

        public Locations Location { get; set; }
        public Users User { get; set; }
        public ICollection<OrderPizza> OrderPizza { get; set; }
    }
}
