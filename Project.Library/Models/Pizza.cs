using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library.Models
{
    public class Pizza
    {
        public int PizzaID { get; set; }
      //  public static decimal BasePrice { get; set; } = 5.00m;
        public decimal Price { get; set; }
        public bool Pepperoni { get; set; }
        public bool ExtraCheese { get; set; }

        public static decimal calculatePrice(bool pep, bool cheese, decimal BasePrice)
        {
            decimal Price = BasePrice;
            Price = pep ? (Price + 1.00m) : Price; //update price if pepperoni is true
            Price = cheese ? (Price + 1.00m) : Price; //update price if cheese is true
            return Price;
        }

        public static int FindPizzaId(bool pep, bool cheese)
        {
            int Id;
            if (!pep & !cheese)
            {
                return Id = 1;
            }
            else if (!pep & !cheese)
            {
                return Id = 2;
            }
            else if (pep & !cheese)
            {
                return Id = 3;
            }
            else
            {
                return Id = 4;
            }
        }
    }
}
