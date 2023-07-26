using System;
using System.Text.RegularExpressions;
namespace Warehouse_Application
{
	public static class ReportMethods
	{
        public static void RaportOfProducts(List<Product> products)
        {
            bool endOfRaport = false;
            do
            {
                Console.Clear();
                Console.WriteLine("RAPORTS:");
                Console.WriteLine("1.All Products\n2.Searching by id\n3.Sort by values\n4.Exit");
                string answer = Console.ReadLine();
                Console.Clear();
                switch (answer)
                {
                    case "1":
                        AllProductRaport(products);
                        break;
                    case "2":
                        SearchingByInformation(products);
                        break;
                    case "3":
                        SortingByValue(products);
                        break;
                    case "4":
                        endOfRaport = true;
                        break;
                    default:
                        break;

                }
                Console.Clear();
            } while (!endOfRaport);
        }
        private static void AllProductRaport(List<Product> products)
        {
            Console.WriteLine("REPORTS");
            Console.WriteLine("- - - - - - - - -");
            foreach (var product in products)
            {
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: {product.Price}");
                Console.WriteLine($"Quantity: {product.Quantity}");
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Date: {product.date}");
                Console.WriteLine("- - - - - - - - - - - -");
            }
        }
        private static void SearchingByInformation(List<Product> products)
        {

            bool endSearching = false;
            do
            {
                List<Product> copyList = products.ToList();
                Console.Write("Id: ");
                string idSearching = Console.ReadLine();
                if (Regex.IsMatch((idSearching), @"^[A-Za-z]{4}\d{5}&"))
                {
                    copyList = copyList.Where(x => x.Id == idSearching).ToList();
                    if(copyList.Count == 0)
                    {
                        Console.WriteLine("Product\n");
                        foreach (var product in copyList)
                        {
                            Console.WriteLine("Name: " + product.Name);
                            Console.WriteLine("Price: " + product.Price);
                            Console.WriteLine("Quantity: " + product.Quantity);
                            Console.WriteLine("Id: " + product.Id);
                            Console.WriteLine("Date: " + product.date);
                            Console.WriteLine("\nClick enter to continue");
                            Console.ReadKey();
                            endSearching = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Id is not in the database\nClick enter to continue or 0 to exit");
                        string exitOrNot = Console.ReadLine();
                        if (exitOrNot == "0")
                            endSearching = true;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong id (4 Letters and 5 numbers, example: Abcd12345)\nClick enter to continue or 0 to exit");
                    string exitOrNot = Console.ReadLine();
                    if (exitOrNot == "0")
                        endSearching = true;
                }
                
            } while (!endSearching);
        }
        private static void SortingByValue(List<Product> products)
        {
            bool endOfSort = false;
            do
            {
                List<Product> copyList = products.ToList();
                Console.Clear();
                Console.WriteLine("1.Sort by value price\n2.Sort by value quantity\n3.Sort by date\n4.Show Raport");
                string answer = Console.ReadLine();
                Console.Clear();
                switch (answer)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        endOfSort = true;
                        break;
                    default:
                        break;
                }
                endOfSort = true;
            } while (!endOfSort); /// UNION etc. 
        }
    }
}

