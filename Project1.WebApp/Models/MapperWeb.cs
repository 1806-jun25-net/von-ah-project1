using Project1.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public class MapperWeb
    {

        //maps a web user to a library user
        public static User Map(UserWeb user) => new User
        {
           // UserID = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DefaultAddress = user.DefaultAddress
            
        };
        public static UserWeb Map(User user) => new UserWeb
        {
            // UserID = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DefaultAddress = user.DefaultAddress

        };

        //maps a web location to a library location
        public static Location Map(LocationWeb location) => new Location //implement dictionary in library class
        {
            LocationID = location.Id,
            Address = location.Address,
            //PepperoniInventory = location.Inventory["Pepperoni"],
        //    CheeseInventory = location.Inventory["Cheese"]

        };

        public static LocationWeb Map(Location location) => new LocationWeb //implement dictionary in library class
        {
            Id = location.LocationID,
            Address = location.Address,
            //PepperoniInventory = location.Inventory["Pepperoni"],
            //    CheeseInventory = location.Inventory["Cheese"]

        };

        public static Order Map(OrderWeb order) => new Order
        {
            OrderID = order.OrderId,
            LocationId = order.LocationId,            
            UserId = order.UserId,
            //  OrderPizzas = Map(order.OrderPizza), //how do i populate my order with a list of pizzas? pizza orders are just as effective
            OrderTime = order.OrderTime,
            OrderTotalValue = order.TotalPrice

        };
        //maps a libray order to a dbcontext order
        public static OrderWeb Map(Order order) => new OrderWeb
        {
               OrderId = order.OrderID,
            // Location = Map(order.OrderLocation),
          //  LocationId = order.OrderLocation.LocationID,
          //  UserId = order.Purchaser.UserID,
          LocationId = order.LocationId,
            UserId = order.UserId,
            //   User = Map(order.Purchaser),
            //  OrderPizzas = Map(order.OrderPizza),
            OrderTime = order.OrderTime,
            TotalPrice = order.OrderTotalValue

        };

        public static List<UserWeb> Map(List<User> users) => users.Select(Map).ToList();
        public static List<User> Map(List<UserWeb> users) => users.Select(Map).ToList();
        public static List<OrderWeb> Map(List<Order> orders) => orders.Select(Map).ToList();
        public static List<Order> Map(List<OrderWeb> orders) => orders.Select(Map).ToList();
    }
}
