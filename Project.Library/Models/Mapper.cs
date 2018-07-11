using Project1.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1.Library.Models
{
    public class Mapper
    {
        //maps a dbcontext User to our library User
        public static User Map(Context.Models.Users user) => new User
        {
            UserID = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DefaultAddress = user.DefaultAddress,
            ManagerFlag = user.ManagerFlag ?? false
            
        };
        //maps a library User to a dbcontext User
        public static Context.Models.Users Map(User user) => new Context.Models.Users
        {
        //    UserId = user.UserID,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DefaultAddress = user.DefaultAddress
        };
        //maps a dbcontext location to our library location
        public static Location Map(Context.Models.Locations location) => new Location
        {
            LocationID = location.LocationId,
            Address = location.Address,
            CheeseInventory = location.ToppingInventoryCheese,
            PepperoniInventory = location.ToppingInventoryPepperoni

        };
        //maps a library location to a dbcontext location
        public static Context.Models.Locations Map(Location location) => new Context.Models.Locations
        {
            LocationId = location.LocationID,
            Address = location.Address,
            ToppingInventoryPepperoni = location.PepperoniInventory,
            ToppingInventoryCheese = location.CheeseInventory
        };
        //maps a dbcontext Pizza to a library pizza
        public static Pizza Map(Context.Models.Pizzas pizza) => new Pizza
        {
            PizzaID = pizza.PizzaId,
            Price = pizza.Price,
            Pepperoni = pizza.ToppingPepperoni,
            ExtraCheese = pizza.ToppingCheese
        };
        //maps a libray pizza to a dbcontext pizza
        public static Context.Models.Pizzas Map(Pizza pizza) => new Context.Models.Pizzas
        {
            PizzaId = pizza.PizzaID,
            Price = pizza.Price,
            ToppingPepperoni = pizza.Pepperoni,
            ToppingCheese = pizza.ExtraCheese
        };
        //maps a dbcontext order to a library order
        public static Order Map(Context.Models.Orders order) => new Order
        {
            OrderID = order.OrderId,
            OrderLocation = Map(order.Location),
            Purchaser = Map(order.User),
          //  OrderPizzas = Map(order.OrderPizza), //how do i populate my order with a list of pizzas? pizza orders are just as effective
           OrderTime = order.OrderTime,
           OrderTotalValue = order.TotalPrice

        };
        //maps a libray order to a dbcontext order
        public static Context.Models.Orders Map(Order order) => new Context.Models.Orders
        {
        //    OrderId = order.OrderID,
           // Location = Map(order.OrderLocation),
            LocationId = order.OrderLocation.LocationID,
            UserId = order.Purchaser.UserID,
         //   User = Map(order.Purchaser),
            //  OrderPizzas = Map(order.OrderPizza),
            OrderTime = order.OrderTime,
            TotalPrice = order.OrderTotalValue

        };

        //allows you to map a list of users in the library to a list of users in the dbcontext. vice-versa
        public static List<User> Map(IEnumerable<Context.Models.Users> users) => users.Select(Map).ToList();
        public static List<Context.Models.Users> Map(IEnumerable<User> users) => users.Select(Map).ToList();
        //allows you to map a list of locations in the library to a list of locations in the dbcontext. vice-versa
        public static List<Location> Map(IEnumerable<Context.Models.Locations> locations) => locations.Select(Map).ToList();
        public static List<Context.Models.Locations> Map(IEnumerable<Location> locations) => locations.Select(Map).ToList();
        //allows you to map a list of pizzas in the libray to a list of pizzas in the dbcontext. vice-versa
        public static List<Pizza> Map(IEnumerable<Context.Models.Pizzas> pizzas) => pizzas.Select(Map).ToList();
        public static List<Context.Models.Pizzas> Map(IEnumerable<Pizza> pizzas) => pizzas.Select(Map).ToList();
        //allows you to map a list of orders in the library to a list of orders in the dbcontext. vice-versa
        public static List<Order> Map(IEnumerable<Context.Models.Orders> orders) => orders.Select(Map).ToList();
        public static List<Context.Models.Orders> Map(IEnumerable<Order> orders) => orders.Select(Map).ToList();



    }
}
