using System;
using System.Text.RegularExpressions;
namespace Warehouse_Application
{
	public static class ReportMethods
	{
        public static void ReportOfProducts(List<Product> products,string systemOp)
        {
            bool endOfRaport = false;
            do
            {
                Console.Clear();
                Console.WriteLine("REPORTS:");
                Console.WriteLine("1.All Products\n2.Searching by id\n3.Sort by values\n4.Exit");
                string answer = Console.ReadLine();
                Console.Clear();
                switch (answer)
                {
                    case "1":
                        AllProductReport(ref products,systemOp);
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
            } while (!endOfRaport);
        }
        private static void AllProductReport(ref List<Product> products,string systemOp)
        {
            bool endOfReport = false;
            do
            {
                string report = "";
                Console.WriteLine("REPORTS");
                Console.WriteLine("- - - - - - - - -");
                foreach (var product in products)
                {
                    report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.date}\n - - - - - - - - \n";
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Price: {product.Price}");
                    Console.WriteLine($"Quantity: {product.Quantity}");
                    Console.WriteLine($"Id: {product.Id}");
                    Console.WriteLine($"Date: {product.date}");
                    Console.WriteLine("- - - - - - - - - - - -");
                }
                Console.WriteLine("\n\n");
                Console.WriteLine("1.Record to txt file\n2.Remove product\n3.Exit");

                Console.Write($"Number: ");
                string answer = Console.ReadLine();

                switch(answer)
                {
                    case "1":
                        Utils.RecordingTxtFile(systemOp, report);
                        break;
                    case "2":
                        Utils.RemovingRecord(ref products,systemOp);
                        break;
                    case "3":
                        endOfReport = true;
                        break;
                    default:
                        break;


                }
            } while (!endOfReport);

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

