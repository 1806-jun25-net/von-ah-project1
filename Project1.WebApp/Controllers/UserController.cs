﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project1.Library.Repositories;
using Project1.WebApp.Models;
using Lib = Project1.Library.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project1.WebApp.Controllers
{
    public class UserController : Controller
    {
        public Project1Repository Repo { get; }

        public MapperWeb Mapper { get; }

        public UserController(Project1Repository repo, MapperWeb mapper)
        {
            Repo = repo;
            Mapper = mapper;
        }

   
        // GET: /<controller>/
        public IActionResult Index()
        {
           // UserWeb user = new UserWeb();
            
            return View();
        }
        public IActionResult ExistingUser()
        {
            UserWeb user = new UserWeb();

            return View(user);
        }
        public IActionResult NewUser()
        {
            UserWeb user = new UserWeb();

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserWeb user)
        {
            try
            {
                var libUser = MapperWeb.Map(user);
                var checkUser = Repo.FindUserId(libUser.FirstName, libUser.LastName);
                if (checkUser == 0)
                {
                    Repo.AddUser(libUser);
                    Repo.Save();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User already exists.");
                    return View(user);
                    
                }
                // TODO: Add insert logic 
            }
            catch
            {
                return View(user);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExistingUser(UserWeb user)
        {
            try
            {
                var libUser = MapperWeb.Map(user);
                var checkUser = Repo.FindUserId(libUser.FirstName, libUser.LastName);
                if (checkUser > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User doesn't exist. Please login as a new user or try again");
                    return View(user);

                }
                // TODO: Add insert logic 
            }
            catch
            {
                return View();
            }
        }


    }
}