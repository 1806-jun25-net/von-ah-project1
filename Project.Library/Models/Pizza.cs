using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library.Models
{
    public class Pizza
    {
        public int PizzaID { get; set; }
        public decimal Price { get; set; }
        public bool Pepperoni { get; set; }
        public bool ExtraCheese { get; set; }

        public static decimal calculatePrice(bool pep, bool cheese, decimal BasePrice)
        {
            decimal CalcPrice = BasePrice;
            CalcPrice = pep ? (CalcPrice + 1.00m) : CalcPrice; //update price if pepperoni is true
            CalcPrice = cheese ? (CalcPrice + 1.00m) : CalcPrice; //update price if cheese is true
            return CalcPrice;
        }

    }
}
