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
            OrderWeb order = new OrderWeb();
            return View(order);
        }
        public ActionResult PlaceOrder(string numP, string loc)
        {
            TempData["numP"] = numP;
            TempData["loc"] = loc;
            OrderWeb order = new OrderWeb
            {
                PizzaCountInt = Convert.ToInt32(numP)
            };


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
            return View(WebOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
                
        public ActionResult Index(OrderWeb order)
        {
         //   var CurrentOrder = new OrderWeb { PizzaCount = order.PizzaCount, Address = order.Address };
            //  Session["Order"] = CurrentOrder;
          //  HttpContext.Session.Set("order", CurrentOrder);



            //if user exists and no datetime conflicts
            return RedirectToAction("PlaceOrder", "Order", new { numP = order.PizzaCountString, loc = order.Address }); //redirect to next part of the order with the data you have. PizzaCount and location (which can make a location id)
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

            order.LocationId = Repo.GetLocationIdByName(order.Address);
            order.OrderTime = DateTime.Now;
            order.UserId = (int)TempData.Peek("id"); //fix add functionality to check if user is logged in
            for (int x = 0; x <order.PizzaCountInt; x++)
            {
                order.PizzaIDs.Add(Repo.FindPizzaIdByToppings(order.PizzaDictionary[x]));
            }
            decimal runningTotal = 0.00m;
            foreach(var id in order.PizzaIDs)
            {
                runningTotal += Repo.FindPriceByPizzaID(id);
            }
            order.TotalPrice = runningTotal;
            var libOrder = MapperWeb.Map(order);
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