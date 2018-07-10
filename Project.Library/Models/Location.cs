using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string Address { get; set; }
  //      public List<KeyValuePair<string, int>> Inventory { get; set; }
     //   public List<Order> OrderHistory { get; set; }
        public int PepperoniInventory { get; set; }
        public int CheeseInventory { get; set; }
  /*      public Location(string address)
        {
            this.Address = address;
        }*/

        public static int FindLocationId(string s)
        {
            int Id;
            if (s.Equals("123 Grove St."))
            {
                return Id = 1; 
            }
            else if (s.Equals("21 Jump St."))
            {
                return Id = 2;
            }
            else
            {
                return Id = 3;
            }
        }
    }
}
