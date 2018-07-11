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
            decimal BasePrice = 5.00m;
            Console.WriteLine("Welcome to my pizza store!");
            Console.WriteLine("Please enter your First Name:");
            var currentUser = new User();
            string fName = Console.ReadLine();
            Console.WriteLine("Please enter your Last Name:");
            string lName = Console.ReadLine();
            Console.WriteLine("");
            int checkInt = repository.FindUserId(fName, lName); //checks if ID exists returns id (which will be greater than 0), if not returns 0
            //does user exist 
            if (checkInt > 0)
            {
                Console.WriteLine("Welcome back {0} {1}", fName, lName);
                currentUser = repository.GetUserByName(fName, lName);
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
                    { //make into a function fix
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
                Console.WriteLine("5. Switch users");
                Console.WriteLine("6. Switch locations");
                if(currentUser.ManagerFlag)
                {
                    Console.WriteLine("7. Change Inventory");
                    Console.WriteLine("8. Change Pizza Base Price");
                }
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
                    bool CancelOrder = false;
                    TimeSpan span;
                    var currentTime = DateTime.Now; //check users last order from location
                    List<Order> userHistory = Order.CreateUserOrderHistory(ListOfOrders, currentUser.FirstName, currentUser.LastName);
                    List<Order> userHistoryAtLocation = Order.CreateLocationOrderHistory(userHistory, currentUser.DefaultAddress);
                    if (userHistoryAtLocation.Count > 0)
                    {
                        var LastOrder = Order.FindLastOrderFromUserFromLocation(userHistoryAtLocation, currentUser.DefaultAddress);
                        span = currentTime.Subtract(LastOrder.OrderTime);
                    }
                    else
                    {
                        span = new TimeSpan(3, 0, 0);
                    }
                    if (span.TotalHours < 2)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("You have placed an order at this location within the last 2 hours. Please try again later");
                        CancelOrder = true;
                    }





                    if (!CancelOrder)
                    {
                        if (userHistory.Count > 0) //checks to see userhistory exists
                        {
                            var LastOrder = Order.FindLastOrderFromUserFromLocation(userHistory, currentUser.DefaultAddress);
                            int i = 1;
                            Console.WriteLine("");
                            Console.WriteLine("Suggested order! The last time you were here you ordered:");
                            foreach (var pizza in LastOrder.OrderPizzas)
                            {
                                Console.WriteLine("Pizza {0} - Price: ${1}", i, pizza.Price);
                                Console.WriteLine("Toppings - Pepperoni: {0}, Extra Cheese: {1}", pizza.Pepperoni.ToString(), pizza.ExtraCheese.ToString());
                                Console.WriteLine("");
                                i++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("You have never orded from this location. We have no suggested order.");
                        }
                        Console.WriteLine("");
                        Console.WriteLine("How many pizzas would you like to order?");
                        //read integer to create pizza List size
                        //check if number of pizzas is more than 12
                        string numOfPizzas = Console.ReadLine();
                        int numOfPizzasInt = 0;
                        var ListOfLocations = new List<Location>();


                        //convert to method fix


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
                        var LocationHistory = Order.CreateLocationOrderHistory(ListOfOrders, currentUser.DefaultAddress);
                        Location currentLocation;
                        /*var currentLocation = new Location
                        {
                            //change this implementation fix
                            LocationID = Location.FindLocationId(currentUser.DefaultAddress),
                            Address = currentUser.DefaultAddress

                        };*/
                        if (LocationHistory.Count > 0)
                        {
                            //  LocationHistory.Sort((x, y) => x.OrderTime.CompareTo(y.OrderTime)); //sorts location history with most recent last
                            //  currentLocation = LocationHistory.Last().OrderLocation; //check this that it actually has correct id fix
                            currentLocation = repository.GetLocationByName(currentUser.DefaultAddress); //gets location from db by address
                        }
                        else
                        {
                            currentLocation = new Location
                            {
                                //change this implementation fix

                                Address = currentUser.DefaultAddress,
                                PepperoniInventory = 50,
                                CheeseInventory = 50
                               
                            }; 
                        }
                        currentLocation.LocationID = Location.FindLocationId(currentUser.DefaultAddress);
                        //Console.WriteLine("{0}", currentLocation.LocationID);
                        decimal runningTotalPrice = 0.00m;
                        for (int i = 0; i < numOfPizzasInt; i++)
                        {       
                            Pizza currentPizza = new Pizza();
                            Console.WriteLine("Pizza #{0}:", i + 1);
                            //sets values for pizza object
                            currentPizza.Pepperoni = ToppingUserChoice("Pepperoni");
                            if (currentPizza.Pepperoni)
                            {
                                currentLocation.PepperoniInventory -= 1;
                                if (currentLocation.PepperoniInventory < 0)
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine("This location is out of Pepperoni");
                                    CancelOrder = true;
                                    break;
                                }
                            }
                            currentPizza.ExtraCheese = ToppingUserChoice("Extra Cheese");
                            if (currentPizza.ExtraCheese)
                            {
                                currentLocation.CheeseInventory -= 1;
                                if (currentLocation.CheeseInventory < 0)
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine("This location is out of Cheese");
                                    CancelOrder = true;
                                    break;
                                }
                            }
                            //adjust location inventory and cancel order fix (break loop and add conditional)
                            currentPizza.PizzaID = Pizza.FindPizzaId(currentPizza.Pepperoni, currentPizza.ExtraCheese);
                            currentPizza.Price = Pizza.calculatePrice(currentPizza.Pepperoni, currentPizza.ExtraCheese, BasePrice); //fix this so price is recieved from database
                            runningTotalPrice += currentPizza.Price; //updates running order total
                            if ((Decimal.Compare(runningTotalPrice, 500.00m)) > 0)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Price has exceeded $500.");
                                CancelOrder = true;
                                break;
                            }
                            orderPizzaList.Add(currentPizza); //updates pizza list
                        }
                        if (!CancelOrder)
                        {

                            int lastOrderId = 0;
                            //find last orderID
                            //the ID of the last order made
                            if(ListOfOrders.Count > 0)
                            {
                                lastOrderId = ListOfOrders.Last().OrderID;
                            }


                            currentUser.UserID = repository.FindUserId(currentUser.FirstName, currentUser.LastName); //adds userID to current user object so order can map

                            currentOrder.SetOrder(currentLocation, currentUser, orderPizzaList, currentTime, runningTotalPrice);
                            currentOrder.OrderID = lastOrderId+1;//sets order object
                            ListOfOrders.Add(currentOrder); //updates order history


                            var currentOrderList = new List<Order>
                            {
                                currentOrder
                            }; //create list of orders to print. no method to print 1 order? fix?
                            Console.WriteLine("");
                            Console.WriteLine("Details about your order.");
                            PrintAListOfOrders(currentOrderList);
                            repository.UpdateLocationInventory(currentLocation); //updates location table's inventory
                            repository.Save();
                            repository.AddOrder(currentOrder);
                            repository.Save();
                            foreach (var pizza in orderPizzaList)
                            {
                                repository.AddOrderPizzas(lastOrderId + 1, pizza.PizzaID);
                                repository.Save();//increases last order ID by 1 and stores values to order pizza table
                            }

                            Serialization.SerializeToFile(@"C:\Revature\data.xml", ListOfOrders); //updates xml document
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Your order has been canceled.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Your order has been canceled.");
                    }
                   

                }
                else if (input == "2") //order history by user
                {

                    Console.WriteLine("Please enter the first name of the user:");
                    string searchFirstName = Console.ReadLine();
                    Console.WriteLine("Please enter the last name of the user:");
                    string searchLastName = Console.ReadLine();

                    //     var userHistory = new List<Order>(); // creates new list of orders to represent the user history
                    List<Order> userHistory = Order.CreateUserOrderHistory(ListOfOrders, searchFirstName, searchLastName);
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
                    List<Order> locationHistory = Order.CreateLocationOrderHistory(ListOfOrders, locationAddress);
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
                else if (input == "4") //reads all users from database now
                {
                    Console.WriteLine("");
                    Console.WriteLine("Please enter the user's first name:");
                    var searchName = Console.ReadLine();
                    var searchUser = repository.SearchUserByFirstName(searchName);
              //      var searchUser = new List<User>();
               /*     foreach(var order in ListOfOrders)
                    {
                        if ((order.Purchaser.FirstName.Equals(searchName)) && (!searchUser.Exists(x => x.FirstName.Equals(order.Purchaser.FirstName) 
                        && (x.LastName.Equals(order.Purchaser.LastName))))) //search to see if the order history has a matched user and that user doesnt exist in the search already
                        {
                            var newSearchUser = new User { FirstName = order.Purchaser.FirstName, LastName = order.Purchaser.LastName, DefaultAddress = "" };
                            searchUser.Add(newSearchUser);
                        }
                    }*/
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
                else if (input == "5")
                {
                    Console.WriteLine("Please enter your First Name:");
                    currentUser = new User();
                    fName = Console.ReadLine();
                    Console.WriteLine("Please enter your Last Name:");
                    lName = Console.ReadLine();
                    Console.WriteLine("");
                    checkInt = repository.FindUserId(fName, lName); //checks if ID exists returns id (which will be greater than 0), if not returns 0
                                                                        //does user exist 
                    if (checkInt > 0)
                    {
                        Console.WriteLine("Welcome back {0} {1}", fName, lName);
                        currentUser = repository.GetUserByName(fName, lName);
                        //find user in order history?
                    }
                    else
                    {
                        //sets default location to one of 3 options
                        while (true)
                        {
                            Console.WriteLine("Welcome new user, Please set default location");
                            Console.WriteLine("1. 123 Grove St.");
                            Console.WriteLine("2. 21 Jump St.");
                            Console.WriteLine("3. 221B Baker St.");
                            //string newUserLocation = Console.ReadLine(); //this allows user to input any location. check this
                            input = Console.ReadLine();
                            string newUserLocation = "";
                            if (input == "1")
                            { //make into a function fix
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
                }
                else if (input == "6")
                {
                    while (true)
                    {
                        Console.WriteLine("Please choose your new location");
                        Console.WriteLine("1. 123 Grove St.");
                        Console.WriteLine("2. 21 Jump St.");
                        Console.WriteLine("3. 221B Baker St.");
                        //string newUserLocation = Console.ReadLine(); //this allows user to input any location. check this
                        input = Console.ReadLine();
                        string newUserLocation = "";
                        if (input == "1")
                        { //make into a function fix
                            newUserLocation = "123 Grove St.";
                            currentUser.DefaultAddress = newUserLocation;
                            //maybe update to database later
                            break;
                        }
                        else if (input == "2")
                        {
                            newUserLocation = "21 Jump St.";
                            currentUser.DefaultAddress = newUserLocation;
                            break;
                        }
                        else if (input == "3")
                        {
                            newUserLocation = "221B Baker St.";
                            currentUser.DefaultAddress = newUserLocation;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please choose an option 1-3");
                        }

                    }
                }
                else if (input == "7" && currentUser.ManagerFlag) //update inventory. works
                {
                    //insert code here to change inventory
                    Console.WriteLine("Which location should update its inventory?");
                    Console.WriteLine("1. 123 Grove St.");
                    Console.WriteLine("2. 21 Jump St.");
                    Console.WriteLine("3. 221B Baker St.");
                    input = Console.ReadLine();
                    if (input == "1") //grove st.
                    { //turn into function
                        var UpdateLocation = new Location { LocationID = 1, Address = "123 Grove St." };
                        Console.WriteLine("What is the new inventory for Pepperoni");
                        var NumOfPepperoni = Console.ReadLine();
                        int IntNumOfPepperoni = 0;
                        int IntNumOfCheese = 0;
                        try
                        {
                            IntNumOfPepperoni = Convert.ToInt32(NumOfPepperoni);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please try a number");
                            NumOfPepperoni = Console.ReadLine();
                        }

                        Console.WriteLine("What is the new inventory for Cheese");
                        var NumOfCheese = Console.ReadLine();
                        try
                        {
                             IntNumOfCheese = Convert.ToInt32(NumOfCheese);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please try a number");
                            NumOfCheese = Console.ReadLine();
                        }
                        UpdateLocation.PepperoniInventory = IntNumOfPepperoni;
                        UpdateLocation.CheeseInventory = IntNumOfCheese;
                        repository.UpdateLocationInventory(UpdateLocation);
                        repository.Save();



                    }
                    else if (input == "2") //jump st.
                    {
                        var UpdateLocation = new Location { LocationID = 2, Address = "21 Jump St." };
                        Console.WriteLine("What is the new inventory for Pepperoni");
                        var NumOfPepperoni = Console.ReadLine();
                        int IntNumOfPepperoni = 0;
                        int IntNumOfCheese = 0;
                        try
                        {
                            IntNumOfPepperoni = Convert.ToInt32(NumOfPepperoni);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please try a number");
                            NumOfPepperoni = Console.ReadLine();
                        }

                        Console.WriteLine("What is the new inventory for Cheese");
                        var NumOfCheese = Console.ReadLine();
                        try
                        {
                            IntNumOfCheese = Convert.ToInt32(NumOfCheese);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please try a number");
                            NumOfCheese = Console.ReadLine();
                        }
                        UpdateLocation.PepperoniInventory = IntNumOfPepperoni;
                        UpdateLocation.CheeseInventory = IntNumOfCheese;
                        repository.UpdateLocationInventory(UpdateLocation);
                        repository.Save();

                    }
                    else if (input == "3") //baker st.
                    {
                        var UpdateLocation = new Location { LocationID = 3, Address = "221B Baker St." };
                        Console.WriteLine("What is the new inventory for Pepperoni");
                        var NumOfPepperoni = Console.ReadLine();
                        int IntNumOfPepperoni = 0;
                        int IntNumOfCheese = 0;
                        try
                        {
                            IntNumOfPepperoni = Convert.ToInt32(NumOfPepperoni);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please try a number");
                            NumOfPepperoni = Console.ReadLine();
                        }

                        Console.WriteLine("What is the new inventory for Cheese");
                        var NumOfCheese = Console.ReadLine();
                        try
                        {
                            IntNumOfCheese = Convert.ToInt32(NumOfCheese);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please try a number");
                            NumOfCheese = Console.ReadLine();
                        }
                        UpdateLocation.PepperoniInventory = IntNumOfPepperoni;
                        UpdateLocation.CheeseInventory = IntNumOfCheese;
                        repository.UpdateLocationInventory(UpdateLocation);
                        repository.Save();
                    }
                    else
                    {
                        Console.WriteLine("Please choose an option 1-3");
                    }
                }
                else if (input == "8" && currentUser.ManagerFlag)
                {
                    //insert code here to change pizza base price
                    Console.WriteLine("");
                    Console.WriteLine("Enter a new price");
                    string price = Console.ReadLine();
                    decimal NewPrice = Convert.ToDecimal(price);
                    BasePrice = NewPrice;
                }
                else
                {
                    Console.WriteLine("");
                    if (!currentUser.ManagerFlag)
                    {
                        Console.WriteLine("Please enter an option 0-6"); //update this for new options
                    }
                    else
                    {
                        Console.WriteLine("Please enter an option 0-8");
                    }
 
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
