using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project1.Library.Repositories;
using Project1.WebApp.Models;

namespace Project1.WebApp.Controllers
{
    public class OrderController : Controller
    {
        public Project1Repository Repo { get; }

        public MapperWeb WebMap { get; }

        

        public OrderController(Project1Repository repo, MapperWeb mapper)
        {
            Repo = repo;
            WebMap = mapper;
        }
        // GET: Order
        public ActionResult Index()
        {
            /*if (TempData.Peek("id").Equals(null)) //redirect to login
            {
                RedirectToAction("Index", "User");
            }*/
            OrderWeb order = new OrderWeb();
            //suggested order logic
    


            return View(order);
        }
        public ActionResult PlaceOrder(string numP, string loc, int lastId)
        {
            TempData["numP"] = numP;
            TempData["loc"] = loc;
            OrderWeb order = new OrderWeb
            {
                PizzaCountInt = Convert.ToInt32(numP)
            };

            order.OrderId = lastId;
            //logic to display suggested order
            //need parameter last order id
            //if order history doesnt exist then lastId will be 0
            //take id and get order
            if (order.OrderId != 0 )
            {

                //grab pizza ids from order pizza table
                List<Library.Models.Pizza> pizzaList = Repo.FindPizzasInOrderPizzaByOrderID(order.OrderId);
                order.PizzaCountDetails = pizzaList.Count();

                //fill out pizzadictionary
                List<bool> ToppingList = new List<bool>();
                ToppingList.Add(false);
                ToppingList.Add(false);
                for (int i = 0; i < order.PizzaCountDetails; i++)
                {
                    order.PizzaDictionary.Add(i, ToppingList);
                }

                for (int i = 0; i < order.PizzaCountDetails; i++)
                {
                    List<bool> OrderToppingList = new List<bool>();
                    OrderToppingList.Add(pizzaList[i].Pepperoni);
                    OrderToppingList.Add(pizzaList[i].ExtraCheese);
                    order.PizzaDictionary[i] = OrderToppingList;                   
                }

                return View(order);
                //develop logic to grab pizza count from order pizza table
                
                //create pizza dictionary from count and ids
                //put logic in html to print pizzas based on dictionary
                //add this logic to order details and maybe order history by location and user
            }
            //fill out details. (details will be overwritten in post action or because they arent part of the form they go away?)


            return View(order);
        }

        public ActionResult OrderDetails(int orderId)
        {
            //get order details by id
            var LibOrder = Repo.GetOrderById(orderId);
            
            var WebOrder = MapperWeb.Map(LibOrder);
            string FirstName = Repo.FindFirstNameById(WebOrder.UserId);
            string LastName = Repo.FindLastNameById(WebOrder.UserId);
            WebOrder.Address = Repo.GetLocationNameById(WebOrder.LocationId);
            WebOrder.UserName = FirstName + " " + LastName;
            List<Library.Models.Pizza> pizzaList = Repo.FindPizzasInOrderPizzaByOrderID(WebOrder.OrderId);
            WebOrder.PizzaCountDetails = pizzaList.Count();

            //fill out pizzadictionary
            List<bool> ToppingList = new List<bool>();
            ToppingList.Add(false);
            ToppingList.Add(false);
            for (int i = 0; i < WebOrder.PizzaCountDetails; i++)
            {
                WebOrder.PizzaDictionary.Add(i, ToppingList);
            }

            for (int i = 0; i < WebOrder.PizzaCountDetails; i++)
            {
                List<bool> OrderToppingList = new List<bool>();
                OrderToppingList.Add(pizzaList[i].Pepperoni);
                OrderToppingList.Add(pizzaList[i].ExtraCheese);
                WebOrder.PizzaDictionary[i] = OrderToppingList;
            }
            return View(WebOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index(OrderWeb order)
        {
            //   var CurrentOrder = new OrderWeb { PizzaCount = order.PizzaCount, Address = order.Address };
            //  Session["Order"] = CurrentOrder;
            //  HttpContext.Session.Set("order", CurrentOrder);



            if (TempData.Peek("id") == null) //redirect to login
            {
                ModelState.AddModelError("", "Please login before placing an order");
                return View(order);
            }
            else
            {
                //datetime logic
                var currentTime = DateTime.Now;
                TimeSpan span;
                var OrderList = Repo.GetOrders();
                var userId = (int)TempData.Peek("id");
                string FirstName = Repo.FindFirstNameById(userId);
                string LastName = Repo.FindLastNameById(userId);
                var userHistory = Library.Models.Order.CreateUserOrderHistory(OrderList, FirstName, LastName);
                var userHistoryAtLocation = Library.Models.Order.CreateLocationOrderHistory(userHistory, order.Address);
                if (userHistoryAtLocation.Count > 0)
                {
                    var LastOrder = Library.Models.Order.FindLastOrderFromUserFromLocation(userHistoryAtLocation);
                    span = currentTime.Subtract(LastOrder.OrderTime);
                    if (span.TotalHours < 2)
                    {
                        ModelState.AddModelError("", "Sorry you cannot place an order from the same location within two hours");
                        return View(order);
                        
                    }
                }
                //pass last order id and print suggested order on place order
                //get last order logic
                if (userHistory.Count > 0) //if user has an order history
                {
                    var LastOrder = Library.Models.Order.FindLastOrderFromUserFromLocation(userHistory);
                    return RedirectToAction("PlaceOrder", "Order", new { numP = order.PizzaCountString, loc = order.Address, lastId = LastOrder.OrderID });
                }
                else
                {
                    return RedirectToAction("PlaceOrder", "Order", new { numP = order.PizzaCountString, loc = order.Address, lastId = 0 });
                }
                 //redirect to next part of the order with the data you have. PizzaCount and location (which can make a location id)


            }

            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(string[] ToppingListForm, OrderWeb order)
        {
            string numP = (string)TempData.Peek("numP");
            string loc = (string)TempData.Peek("loc");
            order.PizzaCountString = numP; //this is a string need to convert to int
            order.Address = loc;
            order.PizzaCountInt = Convert.ToInt32(numP);         
            List<bool> ToppingList = new List<bool>();
            for (int i = 0; i < order.NumOfToppings; i++)
            {
                ToppingList.Add(false);
            }
            for (int i = 0; i < order.PizzaCountInt; i++)
            {
                order.PizzaDictionary.Add(i, ToppingList);
            }



            int boolIndex = 0;
            for (int x = 0; x < order.PizzaCountInt; x++)
            {
                List<bool> OrderToppingList = new List<bool>();
                for (int i = 0; i < order.NumOfToppings; i++)
                {
                      if (ToppingListForm[boolIndex] == "true")
                      {
                          OrderToppingList.Add(true);
                          boolIndex++;
                      }
                      else
                      {
                          OrderToppingList.Add(false);
                      }
                    boolIndex++;
                }
                order.PizzaDictionary[x] = OrderToppingList;
            }
            //inventory logic
            order.LocationId = Repo.GetLocationIdByName(order.Address);
            //get inventory from db by location id (for pizza and cheese)
            var PepperoniInventory = Repo.GetLocationPepperoniInventoryById(order.LocationId);
            var CheeseInventory = Repo.GetLocationCheeseInventoryById(order.LocationId);
            for (int x = 0; x < order.PizzaCountInt; x++)
            {
                order.PizzaIDs.Add(Repo.FindPizzaIdByToppings(order.PizzaDictionary[x]));
            }
            //subtract for each topping on each pizza
            for (int x = 0; x < order.PizzaCountInt; x++) //pepperoni first
            {
                if (order.PizzaDictionary[x][0])
                {
                    PepperoniInventory -= 1;
                }
                if (order.PizzaDictionary[x][1])
                {
                    CheeseInventory -= 1;
                }
            }
            //check if less than 0
            if ((PepperoniInventory < 0) || (CheeseInventory < 0))
            {
                ModelState.AddModelError("", "Sorry we do not have enough inventory at this location to create your order");
                return View(order);
            }
            //if less than 0 add it back and return view
            //ModelState.AddModelError("", "Sorry we do not have enough inventory at this location to create your order");
            //return View(order);


            order.OrderTime = DateTime.Now;
            order.UserId = (int)TempData.Peek("id"); 
            
            decimal runningTotal = 0.00m;
            foreach(var id in order.PizzaIDs)
            {
                runningTotal += Repo.FindPriceByPizzaID(id);
            }
            order.TotalPrice = runningTotal;
            if ((Decimal.Compare(runningTotal, 500.00m)) > 0) //compare to $500
            {
                ModelState.AddModelError("", "Sorry we cannot accept your order, it exceeds $500");
                return View(order);
            }
            var libOrder = MapperWeb.Map(order);
            var location = new Library.Models.Location
            {
                LocationID = order.LocationId,
                Address = Repo.GetLocationNameById(order.LocationId),
                PepperoniInventory = PepperoniInventory,
                CheeseInventory = CheeseInventory
            };
            Repo.UpdateLocationInventory(location);
            Repo.Save();
            Repo.AddOrder(libOrder);
            Repo.Save();
            order.OrderId = Repo.GetOrderIdByDateTime(order.OrderTime);
            foreach (var id in order.PizzaIDs)
            {
                Repo.AddOrderPizzas(order.OrderId, id);
            }
            Repo.Save();


            return RedirectToAction("OrderDetails", "Order", new { orderId = order.OrderId });
        }
    }
}