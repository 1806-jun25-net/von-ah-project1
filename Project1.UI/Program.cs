using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project1.Library;
using Project1.Library.Models;
using Project1.Library.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using P1M = Project1.Context.Models;

namespace Project1.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = configBuilder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<P1M.Project1Context>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Project1"));
            var options = optionsBuilder.Options;

            var dbContext = new P1M.Project1Context(options);
            var repository = new Project1Repository(dbContext);

            var orderHistory = new List<Order>();
            //   orderHistory = Serialization.DeserializeFromFile(@"C:\Revature\data.xml");
            orderHistory = repository.GetOrders();
            RunConsole(orderHistory, dbContext, repository);

            //          Console.WriteLine("breakpoint here");
           //          FillList(orderHistory);
           //          Serialization.SerializeToFile(@"C:\Revature\data.xml", orderHistory);

        }


        static void RunConsole(List<Order> ListOfOrders, P1M.Project1Context dbContext, Project1Repository repository)
        {

            

          //  repository.Test();


            Console.WriteLine("Welcome to my pizza store!");
            Console.WriteLine("Please enter your First Name:");
            var currentUser = new User();
            string fName = Console.ReadLine();
            Console.WriteLine("Please enter your Last Name:");
            string lName = Console.ReadLine();
            Console.WriteLine("");
            bool check = false;
            foreach (var order in ListOfOrders) //iterates thru all orders
            {
                check = order.checkUserExists(fName, lName);
                if (check) //bad code change
                {
                    currentUser = order.ReturnUser();
                    break;
                } //breaks loop at first instance of user
            }
            //does user exist 
            if (check)
            {
                Console.WriteLine("Welcome back {0} {1}", fName, lName);
                //find user in order history?
            }
            else
            {
                //sets default location to one of 3 options
                while (true) {
                    Console.WriteLine("Welcome new user, Please set default location");
                    Console.WriteLine("1. 123 Grove St.");
                    Console.WriteLine("2. 21 Jump St.");
                    Console.WriteLine("3. 221B Baker St.");
                    //string newUserLocation = Console.ReadLine(); //this allows user to input any location. check this
                    var input = Console.ReadLine();
                    string newUserLocation = "";
                    if (input == "1")
                    { //make into a function
                        newUserLocation = "123 Grove St.";
                        currentUser = new User { FirstName = fName, LastName = lName, DefaultAddress = newUserLocation };
                        repository.AddUser(currentUser);
                        repository.Save();
                        break;
                    }
                    else if (input == "2")
                    {
                        newUserLocation = "21 Jump St.";
                        currentUser = new User { FirstName = fName, LastName = lName, DefaultAddress = newUserLocation };
                        repository.AddUser(currentUser);
                        repository.Save();
                        break;
                    }
                    else if (input == "3")
                    {
                        newUserLocation = "221B Baker St.";
                        currentUser = new User { FirstName = fName, LastName = lName, DefaultAddress = newUserLocation };
                        repository.AddUser(currentUser);
                        repository.Save();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please choose an option 1-3");
                    }
                    
                }
            }
            // if not set  default location
            //currentUser is the user object for here on out




            while (true)
                {
                Console.WriteLine("");
                Console.WriteLine("Choose your option:");
                Console.WriteLine("0. Exit Program");
                Console.WriteLine("1. Order a pizza");
                Console.WriteLine("2. Display order history of a user");
                Console.WriteLine("3. Display order history of a location");
                Console.WriteLine("4. Search users by name");
                Console.WriteLine("");
                var input = Console.ReadLine();
                //sanitize this input
                if (input == "0")
                {
                    dbContext.Dispose();
                    Environment.Exit(0);
                }

                else if (input == "1")
                {
                    //place order (still needs implementation for orders > 500, orders that drain inventory, orders made by same user within 2 hrs)
                    Console.WriteLine("How many pizzas would you like to order?");
                    //read integer to create pizza List size
                    //check if number of pizzas is more than 12
                    string numOfPizzas = Console.ReadLine();
                    int numOfPizzasInt = 0;
                    var ListOfLocations = new List<Location>();


                    //convert to method
                    while (true)
                    {
                        try
                        {
                            numOfPizzasInt = Convert.ToInt32(numOfPizzas);
                            if (numOfPizzasInt > 12 || numOfPizzasInt < 1)
                            {
                                Console.WriteLine("Please input an interger 1-12");
                                numOfPizzas = Console.ReadLine();
                            }
                            else { break; }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please input an interger 1-12");
                            numOfPizzas = Console.ReadLine();
                        }
                    }
                    //Order logic for a number of pizzas
                    Order currentOrder = new Order(); //creates new order
                    var orderPizzaList = new List<Pizza>(); //creates new list of pizzas
                    var currentLocation = new Location
                    {
                        //change this implementation 
                        LocationID = Location.FindLocationId(currentUser.DefaultAddress),
                        Address = currentUser.DefaultAddress

                    };
                    decimal runningTotalPrice = 0.00m;
                    for (int i = 0; i < numOfPizzasInt; i++)
                    {
                        Pizza currentPizza = new Pizza();
                        Console.WriteLine("Pizza #{0}:", i+1);
                        //sets values for pizza object
                        currentPizza.Pepperoni = ToppingUserChoice("Pepperoni");
                        currentPizza.ExtraCheese = ToppingUserChoice("Extra Cheese");
                        currentPizza.PizzaID = Pizza.FindPizzaId(currentPizza.Pepperoni, currentPizza.ExtraCheese);
                        currentPizza.Price = Pizza.calculatePrice(currentPizza.Pepperoni, currentPizza.ExtraCheese); //fix this so price is recieved from database
                        runningTotalPrice += currentPizza.Price; //updates running order total
                        orderPizzaList.Add(currentPizza); //updates pizza list
                    }
                    var currentTime = DateTime.Now;

                    //find last orderID
                    int lastOrderId = ListOfOrders.Last().OrderID; //the ID of the last order made
                    currentUser.UserID = repository.FindUserId(currentUser.FirstName, currentUser.LastName); //adds userID to current user object so order can map

                    currentOrder.SetOrder(currentLocation, currentUser, orderPizzaList, currentTime, runningTotalPrice); //sets order object
                    ListOfOrders.Add(currentOrder); //updates order history


                    var currentOrderList = new List<Order>
                    {
                        currentOrder
                    }; //create list of orders to print. no method to print 1 order? fix?
                    Console.WriteLine("");
                    Console.WriteLine("Details about your order.");
                    PrintAListOfOrders(currentOrderList);
                    repository.AddOrder(currentOrder);
                    repository.Save();
                    foreach (var pizza in orderPizzaList)
                    {
                        repository.AddOrderPizzas(lastOrderId+1, pizza.PizzaID); //increases last order ID by 1 and stores values to order pizza table
                    }
                    repository.Save();
                    
                   // Serialization.SerializeToFile(@"C:\Revature\data.xml", ListOfOrders); //updates xml document

                }
                else if (input == "2") //order history by user
                {

                    Console.WriteLine("Please enter the first name of the user:");
                    string searchFirstName = Console.ReadLine();
                    Console.WriteLine("Please enter the last name of the user:");
                    string searchLastName = Console.ReadLine();
                    var userHistory = new List<Order>(); // creates new list of orders to represent the user history
                    //userHistory = null;
                    foreach (var order in ListOfOrders)
                    {
                        var hasUserOrder = order.checkUserExists(searchFirstName, searchLastName); //goes through each order looking for user name
                        if (hasUserOrder)
                        {
                            userHistory.Add(order);
                        }
                    }
                    if (userHistory.Count > 0)
                    {
                        while (true)
                        {
                            Console.WriteLine("How would you like to sort the User History?");
                            Console.WriteLine("1. Most Recent Order");
                            Console.WriteLine("2. Least Recent Order");
                            Console.WriteLine("3. Most Expensive Order");
                            Console.WriteLine("4. Least Expensive Order");
                            input = Console.ReadLine();
                            if (input == "1") //most recent first
                            {
                                userHistory.Sort((x, y) => y.OrderTime.CompareTo(x.OrderTime)); //magical LINQ sort
                                PrintAListOfOrders(userHistory);
                                break;
                            }
                            else if (input == "2") //least recent first
                            {
                                userHistory.Sort((x, y) => x.OrderTime.CompareTo(y.OrderTime));
                                PrintAListOfOrders(userHistory);
                                break;
                            }
                            else if (input == "3") //most expensive order
                            {
                                userHistory.Sort((x, y) => y.OrderTotalValue.CompareTo(x.OrderTotalValue));
                                PrintAListOfOrders(userHistory);
                                break;
                            }
                            else if (input == "4") //least expensive order
                            {
                                userHistory.Sort((x, y) => x.OrderTotalValue.CompareTo(y.OrderTotalValue));
                                PrintAListOfOrders(userHistory);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please enter an option 1-4");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("User {0} {1} has no records to display.", searchFirstName, searchLastName);
                    }



                }
                else if (input == "3") //order history by location
                {
                    string locationAddress = "";
                    while (true)
                    {
                        Console.WriteLine("Please choose the location:");
                        Console.WriteLine("1. 123 Grove St.");
                        Console.WriteLine("2. 21 Jump St.");
                        Console.WriteLine("3. 221B Baker St.");
                        input = Console.ReadLine();
                        if (input == "1")
                        {
                            locationAddress = "123 Grove St.";
                            break;
                        }
                        else if (input == "2")
                        {
                            locationAddress = "21 Jump St.";
                            break;
                        }
                        else if (input == "3")
                        {
                            locationAddress = "221B Baker St.";
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please choose an option 1-3");
                        }
                    }
                    //userHistory = null;
                    var locationHistory = new List<Order>();
                    foreach (var order in ListOfOrders) //this was copy/paste from user Display. make into a function?
                    {
                        var hasLocationOrder = order.checkLocation(locationAddress);
                        if (hasLocationOrder)
                        {
                            locationHistory.Add(order);
                        }
                    }
                    if (locationHistory.Count > 0)
                    {
                        while (true)
                        {
                            Console.WriteLine("How would you like to sort the Location History?");
                            Console.WriteLine("1. Most Recent Order");
                            Console.WriteLine("2. Least Recent Order");
                            Console.WriteLine("3. Most Expensive Order");
                            Console.WriteLine("4. Least Expensive Order");
                            input = Console.ReadLine();
                            if (input == "1") //most recent first
                            {
                                locationHistory.Sort((x, y) => y.OrderTime.CompareTo(x.OrderTime)); //magical LINQ sort
                                PrintAListOfOrders(locationHistory);
                                break;
                            }
                            else if (input == "2") //least recent first
                            {
                                locationHistory.Sort((x, y) => x.OrderTime.CompareTo(y.OrderTime));
                                PrintAListOfOrders(locationHistory);
                                break;
                            }
                            else if (input == "3") //most expensive order
                            {
                                locationHistory.Sort((x, y) => y.OrderTotalValue.CompareTo(x.OrderTotalValue));
                                PrintAListOfOrders(locationHistory);
                                break;
                            }
                            else if (input == "4") //least expensive order
                            {
                                locationHistory.Sort((x, y) => x.OrderTotalValue.CompareTo(y.OrderTotalValue));
                                PrintAListOfOrders(locationHistory);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please enter an option 1-4");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Location {0} has no records to display.", locationAddress);
                    }
                
                    
                }
                else if (input == "4")
                {
                    Console.WriteLine("");
                    Console.WriteLine("Please enter the user's first name:");
                    var searchName = Console.ReadLine();
                    var searchUser = new List<User>();
                    foreach(var order in ListOfOrders)
                    {
                        if ((order.Purchaser.FirstName.Equals(searchName)) && (!searchUser.Exists(x => x.FirstName.Equals(order.Purchaser.FirstName) 
                        && (x.LastName.Equals(order.Purchaser.LastName))))) //search to see if the order history has a matched user and that user doesnt exist in the search already
                        {
                            var newSearchUser = new User { FirstName = order.Purchaser.FirstName, LastName = order.Purchaser.LastName, DefaultAddress = "" };
                            searchUser.Add(newSearchUser);
                        }
                    }
                    if (searchUser.Count > 0)
                    {
                        PrintAListOfUsers(searchUser);
                    }
                    else
                    {
                        Console.WriteLine("");
                        Console.WriteLine("User {0} doesn't exist in our order history", searchName);
                    }
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Please enter an option 0-4"); //update this for new options
                    Console.WriteLine("");
                }

            }
        }

        public static void PrintAListOfUsers(List<User> users)
        {
            Console.WriteLine("");
            int i = 1;
            foreach(var user in users)
            {
                Console.WriteLine("{0}. {1} {2}", i, user.FirstName, user.LastName);
                i++;     
            }
        }

        public static void PrintAListOfOrders(List<Order> orderHistory)
        {
            foreach (var order in orderHistory)
            {
                Console.WriteLine("User: {0} {1}", order.Purchaser.FirstName, order.Purchaser.LastName);
                Console.WriteLine("Location: {0}", order.OrderLocation.Address);
                Console.WriteLine("");
                int i = 1;
                foreach (var pizza in order.OrderPizzas)
                {
                    Console.WriteLine("Pizza {0} - Price: ${1}", i, pizza.Price);
                    Console.WriteLine("Toppings - Pepperoni: {0}, Extra Cheese: {1}", pizza.Pepperoni.ToString(), pizza.ExtraCheese.ToString());
                    Console.WriteLine("");
                    i++;
                }
                Console.WriteLine("Ordered at {0}, Total Price: ${1}", order.OrderTime, order.OrderTotalValue);
                Console.WriteLine("");
            }
        }

        public static bool ToppingUserChoice(string topping)
        {

            Console.WriteLine("Would you like to add {0} to your pizza? y/n", topping);
            while (true)
            {
                string Answer = Console.ReadLine();
                if (Answer == "y")
                {
                    return true;
                }
                else if (Answer == "n")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("{0} is not a valid answer, please type y/n.", Answer);
                }
            }
        }

  /*      private static void FillList(List<Order> list)
        {
            list.Add(new Order
            {
                OrderLocation = new Location
                {
                    Address = "123 Grove St."
                     Inventory = new List<KeyValuePair<string, int>>() { new KeyValuePair<string,int> ("Pepperoni", 5),
                     new KeyValuePair<string,int>  ("Cheese", 10) } //add inventory back in after xml testing
                },
                Purchaser = new User
                {
                    FirstName = "Lance", LastName = "Von Ah", DefaultAddress = "123 Grove St."

                },
                OrderPizzas = new List<Pizza>
                {
                   new Pizza() { Price = 5.45m,  Pepperoni = true, ExtraCheese = true },
                   new Pizza() { Price = 4.00m, Pepperoni = true, ExtraCheese = false}
                },
                OrderTime = new DateTime(2017, 1, 18),
                OrderTotalValue = 9.45m

            });
        }*/

    }
}
