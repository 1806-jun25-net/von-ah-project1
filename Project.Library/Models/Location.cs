using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string Address { get; set; }
        public Dictionary<string, int> Inventory {get; set;}
        public int PepperoniInventory { get; set; }
        public int CheeseInventory { get; set; }


        
    }
}
