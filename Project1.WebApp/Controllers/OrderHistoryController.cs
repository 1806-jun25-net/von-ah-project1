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
    public class OrderHistoryController : Controller
    {

        public Project1Repository Repo { get; }

        public MapperWeb WebMap { get; }

        public OrderHistoryController(Project1Repository repo, MapperWeb mapper)
        {
            Repo = repo;
            WebMap = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ByUser()
        {
            UserWeb user = new UserWeb();
            return View(user);
        }

        public ActionResult ByLocation()
        {
            LocationWeb location = new LocationWeb();
            return View(location);
        }

        public ActionResult SearchResultsUser(int searchID, string searchOption) //functional
        {
          //  try
          //  {
                var UserHistory = (Repo.GetOrdersByUserId(searchID));
                if (searchOption == "1")
                {
                    UserHistory.Sort((x, y) => y.OrderTotalValue.CompareTo(x.OrderTotalValue));
                }
                else if (searchOption == "2")
                {
                   UserHistory.Sort((x, y) => x.OrderTotalValue.CompareTo(y.OrderTotalValue));
                }
                else if (searchOption == "3") //most recent
                {
                    UserHistory.Sort((x, y) => y.OrderTime.CompareTo(x.OrderTime));
                }
                else //least recent
                {
                    UserHistory.Sort((x, y) => x.OrderTime.CompareTo(y.OrderTime));
                }
                var WebUserHistory = MapperWeb.Map(UserHistory);
                string FirstName = "";
                string LastName = "";
                foreach (var order in WebUserHistory)
                {
                    order.Address = Repo.GetLocationNameById(order.LocationId);
                    FirstName = Repo.FindFirstNameById(order.UserId);
                    LastName = Repo.FindLastNameById(order.UserId);
                    order.UserName = FirstName + " " + LastName;
                }
                return View(WebUserHistory);
           // }
           // catch
           // {
               // return View();
            //}
        }

        public ActionResult SearchResultsLocation(int searchID, string searchOption)
        {
            try
            {
                var LocationHistory = (Repo.GetOrdersByLocationId(searchID));
                if (searchOption == "1") //most expensive
                {
                    LocationHistory.Sort((x, y) => y.OrderTotalValue.CompareTo(x.OrderTotalValue));
                }
                else if (searchOption == "2") //least expensive
                {
                    LocationHistory.Sort((x, y) => x.OrderTotalValue.CompareTo(y.OrderTotalValue));
                }
                else if (searchOption == "3") //most recent
                {
                    LocationHistory.Sort((x, y) => y.OrderTime.CompareTo(x.OrderTime));
                }
                else //least recent
                {
                    LocationHistory.Sort((x, y) => x.OrderTime.CompareTo(y.OrderTime));
                }
                var WebLocationHistory = MapperWeb.Map(LocationHistory);
                foreach (var order in WebLocationHistory)
                {
                    order.Address = Repo.GetLocationNameById(order.LocationId);
                }
                string FirstName = "";
                string LastName = "";
                foreach (var order in WebLocationHistory)
                {
                    order.Address = Repo.GetLocationNameById(order.LocationId);
                    FirstName = Repo.FindFirstNameById(order.UserId);
                    LastName = Repo.FindLastNameById(order.UserId);
                    order.UserName = FirstName + " " + LastName;
                }
                return View(WebLocationHistory);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ByUser(UserWeb user)
        {
            try
            {
                var libUser = MapperWeb.Map(user);
                var checkUser = Repo.FindUserId(libUser.FirstName, libUser.LastName);
                if (checkUser > 0)
                {
                    return RedirectToAction("SearchResultsUser", "OrderHistory", new { searchID = checkUser, searchOption = user.SearchOption}); //add search option
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User doesn't exist. Please retype your search");
                    return View(user);

                }
                // TODO: Add insert logic 
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ByLocation(LocationWeb location)
        {
            try
            {
                var libLocaton = MapperWeb.Map(location);
                var checkLocation = Repo.GetLocationIdByName(location.Address);
                if (checkLocation > 0)
                {
                    return RedirectToAction("SearchResultsLocation", "OrderHistory", new { searchID = checkLocation, searchOption = location.SearchOption }); //add search option
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Location doesn't exist. Please edit your search");
                    return View();

                }
                // TODO: Add insert logic 
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchResultsUser(List<OrderWeb> UserHistory)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchResultsLocation(List<OrderWeb> LocationHistory)
        {
            return View();
        }
    }
}
