using Microsoft.EntityFrameworkCore;
using Project1.Context;
using Project1.Context.Models;
using Project1.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
    