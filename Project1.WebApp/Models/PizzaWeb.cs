using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public class PizzaWeb
    {
        public int PizzaId { get; set; }
        public decimal Price { get; set; }
        public Dictionary<string,bool> Toppings { get; set; }
    }
}
