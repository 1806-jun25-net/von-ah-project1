using System;
using System.Collections.Generic;

namespace Project1.Context.Models
{
    public partial class Pizzas
    {
        public Pizzas()
        {
            OrderPizza = new HashSet<OrderPizza>();
        }

        public int PizzaId { get; set; }
        public bool ToppingPepperoni { get; set; }
        public bool ToppingCheese { get; set; }
        public decimal Price { get; set; }

        public ICollection<OrderPizza> OrderPizza { get; set; }
    }
}
