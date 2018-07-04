using Project1.Library;
using System;
using System.Collections.Generic;

namespace Project1.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var orderHistory = new List<Order>();
            orderHistory = Serialization.DeserializeFromFile(@"C:\Revature\data.xml");
            RunConsole(orderHistory);
            //          Console.WriteLine("breakpoint here");
            //         FillList(list);
            //       Serialization.SerializeToFile(@"C:\Revature\data.xml", list);
        }
        static void RunConsole(List<Order> ListOfOrders)
        {
            while (true)
            {
                Console.WriteLine("Welcome to my pizza store!");
                Console.WriteLine("Please enter your First Name:");

                string fName = Console.ReadLine();
                Console.WriteLine("Please enter your Last Name:");
                string lName = Console.ReadLine();
                Console.WriteLine("");
                bool check = false;
                foreach (var order in ListOfOrders) //iterates thru all orders
                {
                  check = order.checkUserExists(fName, lName);
                  if (check) { break; } //breaks loop at first instance of user
                }
                //does user exist 
                if (check)
                {
                    Console.WriteLine("Welcome back {0} {1}", fName, lName);
                }
                else
                {
                    Console.WriteLine("Welcome new user, Please set default location");
                    string newUserLocation = Console.ReadLine(); //this allows user to input any location. check this
                    var newUser = new User { FirstName = fName, LastName = lName, DefaultAddress = newUserLocation };
                    Console.WriteLine("");
                }
                // if not set  default location





                Console.WriteLine("Choose your option:");
                Console.WriteLine("0. Exit Program");
                Console.WriteLine("1. Order a pizza");
                Console.WriteLine("");
                var input = Console.ReadLine();

                if (input == "0")
                {
                    Environment.Exit(0);
                }

                if (input == "1")
                {
                    //place order
                    Console.WriteLine("How many pizzas would you like to order?");
                    //read integer to create pizza List size
                    //check if number of pizzas is more than 12
                    string numOfPizzas = Console.ReadLine();

                    //convert to method
                    try
                    {
                        int numOfPizzasInt = Convert.ToInt32(numOfPizzas);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Please input an interger 1-12");
                    }
                }

            }
        }

        private static void FillList(List<Order> list)
        {
            list.Add(new Order
            {
                OrderLocation = new Location {
                    Address = "123 Grove St."
                  /*  Inventory = new Dictionary<string,int> {{ "Pepperoni", 5},
                    { "Cheese", 10} }*/ //add inventory back in after xml testing
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
        }

    }
}
