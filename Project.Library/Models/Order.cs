using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Project1.Library.Models
{
    public class Order
    {
        [XmlElement]
        public int OrderID { get; set; }
        public Location OrderLocation { get; set; }
        public User Purchaser { get; set; }
        public List<Pizza> OrderPizzas { get; set; } = new List<Pizza>();
        public DateTime OrderTime { get; set; }
        public decimal OrderTotalValue { get; set; }
        public int LocationId { get; set; }
        public int UserId { get; set; }


        public bool checkUserExists(string fName, string lName) //compares order history user names to user input
        {
            if (this.Purchaser.FirstName.Equals(fName) && this.Purchaser.LastName.Equals(lName))
            {
                return true;
            }
            else { return false; }
        }

        public bool checkLocation(string address)
        {
            if (this.OrderLocation.Address.Equals(address))
            {
                return true;
            }
            else { return false; }
        }
        public User ReturnUser()
        {
            return this.Purchaser;
        }

        public void SetOrder(Location location, User Purchaser, List<Pizza> OrderPizzas, DateTime OrderTime, decimal OrderTotalValue)
        {
            this.OrderLocation = location;
            this.Purchaser = Purchaser;
            this.OrderPizzas = OrderPizzas;
            this.OrderTime = OrderTime;
            this.OrderTotalValue = OrderTotalValue;
        }

        public static List<Order> CreateUserOrderHistory(List<Order> orders, string FirstName, string LastName)
        {


            var userHistory = new List<Order>(); // creates new list of orders to represent the user history

            foreach (var order in orders)
            {
                var hasUserOrder = order.checkUserExists(FirstName, LastName); //goes through each order looking for user name
                if (hasUserOrder)
                {
                    userHistory.Add(order);
                }
            }
            return userHistory;
        }

        public static List<Order> CreateLocationOrderHistory(List<Order> orders, string address)
        {
            var locationHistory = new List<Order>();
            foreach (var order in orders) //this was copy/paste from user Display. make into a function? fix
            {
                var hasLocationOrder = order.checkLocation(address);
                if (hasLocationOrder)
                {
                    locationHistory.Add(order);
                }
            }
            return locationHistory;
        }


        public static Order FindLastOrderFromUserFromLocation(List<Order> orders)
        {
            orders.Sort((x, y) => y.OrderTime.CompareTo(x.OrderTime));
            return orders.FirstOrDefault(); //look at this when list is empty
        }
    }

}

