using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using Project1.Context;
using Project1.Context.Models;
using Project1.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Lib = Project1.Library.Models;

namespace Project1.Library.Repositories
{
    public class Project1Repository
    {
        private readonly Project1Context _db;

        public Project1Repository(Project1Context db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        //add user on new user input
        public void AddUser(Models.User user)
        {
            _db.Add(Mapper.Map(user));
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        //gets a list of all orders
        public List<Models.Order> GetOrders()
        {
            var OrderList = Mapper.Map(_db.Orders.Include(x => x.Location).Include(y => y.User).AsNoTracking());
            foreach (var order in OrderList)
            {
                order.OrderPizzas = FindPizzasInOrderPizzaByOrderID(order.OrderID);
            }
            return OrderList;
            
        }

        public List<Models.Order> GetOrdersByUserId(int id)
        {
            var OrderList = Mapper.Map(_db.Orders.Include(x => x.Location).Include(y => y.User).AsNoTracking());
            var user = FindUserNameById(id);
            List<Lib.Order> UserHistory = Lib.Order.CreateUserOrderHistory(OrderList, user.FirstName, user.LastName);
            return UserHistory;
        }

        public List<Models.Order> GetOrdersByLocationId(int id)
        {
            var OrderList = Mapper.Map(_db.Orders.Include(x => x.Location).Include(y => y.User).AsNoTracking());
            var location = FindLocationById(id);
            List<Lib.Order> LocationHistory = Lib.Order.CreateLocationOrderHistory(OrderList, location.Address);
            return LocationHistory;
        }

        public Models.Order GetOrderById(int id)
        {
            var orders = _db.Orders.Include(x => x.Location).Include(y => y.User).AsNoTracking();
            foreach (var order in orders)
            {
                if(order.OrderId == id)
                {
                    return Mapper.Map(order);
                }
            }
            return null;
        }

        public int GetOrderIdByDateTime(DateTime time)
        {
            var orders = _db.Orders;
            foreach(var order in orders)
            {
                if (order.OrderTime.Equals(time))
                {
                    return order.OrderId;
                }
            }
            return 0;
        } 
      

        public Context.Models.Locations FindLocationById(int id)
        {
            var locations = _db.Locations;
            foreach(var location in locations)
            {
                if (location.LocationId.Equals(id))
                {
                    return location;
                }
                
            }
            return null;
        }

        public Context.Models.Users FindUserNameById(int id)
        {
            var users = _db.Users;
            foreach (var user in users)
            {
                if (user.UserId.Equals(id))
                {
                    return user;
                }
            }
            return null;
        }


        public string FindFirstNameById(int id)
        {
            var users = _db.Users;
            foreach (var user in users)
            {
                if (user.UserId.Equals(id))
                {
                    return user.FirstName;
                }
            }
            return null;
        }
        public string FindLastNameById(int id)
        {
            var users = _db.Users;
            foreach (var user in users)
            {
                if (user.UserId.Equals(id))
                {
                    return user.LastName;
                }
            }
            return null;
        }
        public bool FindManagerFlagById(int id)
        {
            var users = _db.Users;
            foreach (var user in users)
            {
                if (user.UserId.Equals(id))
                {
                    return (bool)user.ManagerFlag;
                }
            }
            return false;
        }


        public int FindPizzaIdByToppings(List<bool> Toppings)
        {
            var pizzas = _db.Pizzas;
            foreach (var pizza in pizzas)
            {
                if ((pizza.ToppingPepperoni == Toppings[0]) && (pizza.ToppingCheese == Toppings[1]))
                {
                    return pizza.PizzaId;
                }
            }
            return 0;
        }

        public decimal FindPriceByPizzaID(int id)
        {
            var pizzas = _db.Pizzas;
            foreach(var pizza in pizzas)
            {
                if(pizza.PizzaId == id)
                {
                    return pizza.Price;
                }
            }
            return 0.00m;
        }

        public Models.User GetUserByName(string First, string Last)
        {
            var users = _db.Users;
            foreach (var user in users)
            {
                if ((user.FirstName.Equals(First)) && (user.LastName.Equals(Last)))
                {
                    return Mapper.Map(user);
                }
            }
            return null;
        }

        public Models.Location GetLocationByName(string address)
        {
            var locations = _db.Locations;
            foreach (var location in locations)
            {
                if (location.Address.Equals(address))
                {
                    return Mapper.Map(location);
                }
            }
            return null;
        }

        public int GetLocationIdByName(string address)
        {
            var locations = _db.Locations;
            foreach (var location in locations)
            {
                if(location.Address.Equals(address))
                {
                    return location.LocationId;
                }
            }
            return 0;
        }
        public string GetLocationNameById(int id)
        {
            var locations = _db.Locations;
            foreach (var location in locations)
            {
                if (location.LocationId.Equals(id))
                {
                    return location.Address;
                }
            }
            return "";
        }

        public int GetLocationPepperoniInventoryById(int id)
        {
            var locations = _db.Locations;
            foreach (var location in locations)
            {
                if (location.LocationId.Equals(id))
                {
                    return location.ToppingInventoryPepperoni;
                }
            }
            return 0;
        }   

        public int GetLocationCheeseInventoryById(int id)
        {
            var locations = _db.Locations;
            foreach (var location in locations)
            {
                if (location.LocationId.Equals(id))
                {
                    return location.ToppingInventoryCheese;
                }
            }
            return 0;
        }

        

        public List<Models.User> SearchUserByFirstName(string First)
        {
            var users = _db.Users;
            var SearchedUsers = new List<Models.User>();
            foreach (var user in users)
            {
                if(user.FirstName.Equals(First))
                {
                    SearchedUsers.Add(Mapper.Map(user));
                }
            }
            return SearchedUsers;
        }

        //map libray order to context order. will need to also update order pizza table. needs to return the new order ID
        public void AddOrder(Models.Order order)
        {
            _db.Add(Mapper.Map(order));
           // Console.WriteLine("{0}", order.OrderID);
        }
        //creates entry in order pizza table
        public void AddOrderPizzas(int orderID, int pizzaId)
        {
            Context.Models.OrderPizza SingleOrderPizza = new Context.Models.OrderPizza { OrderId = orderID, PizzaId = pizzaId };
            _db.Add(SingleOrderPizza);
        }
        //finds user id based on first name last name, if not in table (shouldnt happen) returns 0
       public int FindUserId(string First, string Last)
        {
            var users = _db.Users;
            foreach (var user in users)
            {
                if ((user.FirstName.Equals(First)) && (user.LastName.Equals(Last)))
                {
                    return user.UserId;
                }
            }
            return 0;
        }

        //finds pizzas that have matching order ID
        public List<Models.Pizza> FindPizzasInOrderPizzaByOrderID(int orderId)
        {
            List<Models.Pizza> pizzaList = new List<Models.Pizza>();
            foreach (var orderPizza in _db.OrderPizza)
            {
                if (orderPizza.OrderId == orderId)
                {
                    pizzaList.Add(Mapper.Map(_db.Pizzas.Find(orderPizza.PizzaId)));
                }
            }
            return pizzaList;
        }


        //will update location inventory after an order
        public void UpdateLocationInventory(Models.Location location)
        {
            _db.Entry(_db.Locations.Find(location.LocationID)).CurrentValues.SetValues(Mapper.Map(location));
         /*   _db.Locations.Attach(Mapper.Map(location));
            _db.Entry(_db.Locations.Find(location.LocationID)).Property(x => x.ToppingInventoryCheese).IsModified = true;
            _db.Entry(_db.Locations.Find(location.LocationID)).Property(x => x.ToppingInventoryPepperoni).IsModified = true;*/
        }
       
        /* public void Test()
         {
             foreach (var orderPizza in _db.OrderPizza)
             {
                 //     Console.WriteLine("{0} {1}", orderPizza.OrderId, orderPizza.PizzaId);
                 var pizza = _db.Pizzas.Find(4);
                 Console.WriteLine("{0},{1},{2}", pizza.PizzaId, pizza.Price, pizza.ToppingCheese);
             }
         }*/
    }
}
    