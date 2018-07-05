using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Pizza
    {
        public decimal Price { get; set; }
        public bool Pepperoni { get; set; }
        public bool ExtraCheese { get; set; }

        public static decimal calculatePrice(bool pep, bool cheese)
        {
            decimal Price = 5.00m;
            Price = pep ? (Price + 2.00m) : Price; //update price if pepperoni is true
            Price = cheese ? (Price + 1.00m) : Price; //update price if cheese is true
            return Price;
        }
    }
}
