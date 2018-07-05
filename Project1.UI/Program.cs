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
           //          FillList(orderHistory);
           //          Serialization.SerializeToFile(@"C:\Revature\data.xml", orderHistory);
        }
        static void RunConsole(List<Order> ListOfOrders)
        {
            while (true)
            {
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
                    Console.WriteLine("Welcome new user, Please set default location");
                    string newUserLocation = Console.ReadLine(); //this allows user to input any location. check this
                    currentUser = new User { FirstName = fName, LastName = lName, DefaultAddress = newUserLocation };
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
                    int numOfPizzasInt = 0;

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
                    var currentLocation = new Location();

                    //change this implementation 

                    currentLocation.Address = currentUser.DefaultAddress;
                    decimal runningTotalPrice = 0.00m;
                    for (int i = 0; i < numOfPizzasInt; i++)
                    {
                        Pizza currentPizza = new Pizza();
                        Console.WriteLine("Pizza #{0}:", i+1);
                        //sets values for pizza object
                        currentPizza.Pepperoni = ToppingUserChoice("Pepperoni");
                        currentPizza.ExtraCheese = ToppingUserChoice("Extra Cheese");
                        currentPizza.Price = Pizza.calculatePrice(currentPizza.Pepperoni, currentPizza.ExtraCheese);
                        runningTotalPrice += currentPizza.Price; //updates running order total
                        orderPizzaList.Add(currentPizza); //updates pizza list
                    }
                    var currentTime = DateTime.Now;
                    currentOrder.SetOrder(currentLocation, currentUser, orderPizzaList, currentTime, runningTotalPrice); //sets order object
                    ListOfOrders.Add(currentOrder); //updates order history
                    Serialization.SerializeToFile(@"C:\Revature\data.xml", ListOfOrders); //updates xml document

                }

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

        private static void FillList(List<Order> list)
        {
            list.Add(new Order
            {
                OrderLocation = new Location
                {
                    Address = "123 Grove St."
                    /* Inventory = new List<KeyValuePair<string, int>>() { new KeyValuePair<string,int> ("Pepperoni", 5),
                     new KeyValuePair<string,int>  ("Cheese", 10) }*/ //add inventory back in after xml testing
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
