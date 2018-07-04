﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Project1.Library
{
    public class Order
    {
        [XmlElement]    
        public Location OrderLocation { get; set; }
        public User Purchaser { get; set; }
        public List<Pizza> OrderPizzas = new List<Pizza>();
        public DateTime OrderTime { get; set; }
        public decimal OrderTotalValue { get; set; }


        public bool checkUserExists(string fName, string lName) //compares order history user names to user input
        {
            if (this.Purchaser.FirstName.Equals(fName) && this.Purchaser.LastName.Equals(lName))
            {
                return true;
            }
            else { return false; }
        }
        
    }
  /*  public int checkNumOfOrderedPizzasIsInt(string pizzas)
    {
        try
        {
            int numOfPizzasInt = Convert.ToInt32(pizzas);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Please input an interger 1-12");
        }
    }*/


}