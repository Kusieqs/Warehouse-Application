using System;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Collections.Generic;
namespace Warehouse_Application
{
    public static class ReportMethods
    {
        public static void ReportOfProducts(List<Product> products, string systemOp)
        {
            bool endOfRaport = false;
            do
            {
                Console.Clear();
                Console.WriteLine("REPORTS:");
                Console.WriteLine("1.All Products\n2.Search by id\n3.Sort by values\n4.Exit");
                string answer = Console.ReadLine();
                Console.Clear();
                switch (answer)
                {
                    case "1":
                        AllProductReport(ref products, systemOp);
                        break;
                    case "2":
                        SearchingById(products, systemOp);
                        break;
                    case "3":
                        SortingByValue(products, systemOp);
                        break;
                    case "4":
                        endOfRaport = true;
                        break;
                    default:
                        break;

                }
            } while (!endOfRaport);
        }
        private static void AllProductReport(ref List<Product> products, string systemOp)
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
                ReportMenu(systemOp, report, ref endOfReport);
            } while (!endOfReport);

        }
        private static void SearchingById(List<Product> products, string systemOp)
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
                    if (copyList.Count == 0)
                    {
                        string report = "";
                        Console.WriteLine("Product\n");
                        foreach (var product in copyList)
                        {
                            report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.date}\n - - - - - - - - \n";
                            Console.WriteLine("Name: " + product.Name);
                            Console.WriteLine("Price: " + product.Price);
                            Console.WriteLine("Quantity: " + product.Quantity);
                            Console.WriteLine("Id: " + product.Id);
                            Console.WriteLine("Date: " + product.date);
                            Console.WriteLine("\nClick enter to continue");
                            Console.ReadKey();
                            endSearching = true;
                        }
                        Console.WriteLine("\n\n");
                        ReportMenu(systemOp, report, ref endSearching);

                    }
                    else
                    {
                        Console.WriteLine("Id is not in the database\nClick enter to continue or 0 to exit");
                        string exitOrNot = Console.ReadLine();
                        if (exitOrNot == "0")
                            endSearching = false;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong id (4 Letters and 5 numbers, example: Abcd12345)\nClick enter to continue or 0 to exit");
                    string exitOrNot = Console.ReadLine();
                    if (exitOrNot == "0")
                        endSearching = false;
                }

            } while (!endSearching);
        }
        private static void SortingByValue(List<Product> products, string systemOp)
        {
            List<Product> copyList = new List<Product>();
            List<Product> sortList = new List<Product>();
            bool endOfSort = false, attempt = false;
            object test = null;
            string property = null;
            string report, sortingBy, operatorSort;
            DateTime dateSorting;
            double value = 0;
            int year, month, day;
            bool yearBool = false, monthBool = false, dayBool = false;
            do
            {
                do
                {
                    Console.Clear();
                    Console.Write("1.Sort by value price\n2.Sort by value quantity\n3.Sort by date\n4.Show Raport\n5.Exit\n\nNumber: ");
                    sortingBy = Console.ReadLine();
                    if (sortingBy == "Price" || sortingBy == "Quantity" || sortingBy == "Date")
                        attempt = true;
                } while (!attempt);
                attempt = false;
                do
                {
                    Console.Clear();
                    if(sortingBy == "Date")
                    {
                        do
                        {
                            Console.Clear();
                            Console.Write("Year: ");
                            yearBool = int.TryParse(Console.ReadLine(), out year);
                            Console.Write("Month: ");
                            monthBool = int.TryParse(Console.ReadLine(), out month);
                            Console.Write("Day: ");
                            dayBool = int.TryParse(Console.ReadLine(), out day);
                            if (yearBool && monthBool && dayBool)
                            {
                                if((year < 1 || month < 1 || month > 12 || day < 1))
                                {
                                    int daysInMonth = DateTime.DaysInMonth(year, month);
                                    Console.WriteLine("Wrong Date\nClick enter to continue");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    int daysInMonth = DateTime.DaysInMonth(year, month);
                                    if (daysInMonth >= day)
                                    {
                                        dateSorting = new DateTime(year, month, day);
                                        attempt = true;
                                    }
                                }
                            }
                        } while (!attempt);
                    }
                    else
                    {
                        Console.Write("Value: ");
                        attempt = double.TryParse(Console.ReadLine(), out value);
                    }
                } while (!attempt);
                attempt = false;
                do
                {
                    Console.Clear();
                    Console.Write($"Choose one of this operators ({value} == , != , > , < , >= , <= ): ");
                    operatorSort = Console.ReadLine();
                    string[] operators = new string[] { "==", "!=", ">", "<", ">=", "<=" };
                    for (int i = 0; i < operators.Length; i++)
                    {
                        if (operatorSort == operators[i])
                            attempt = true;
                    }
                } while (!attempt);
                attempt = false;

                //// here to continue; ()

            } while (!endOfSort); /// UNION etc.
            /// show
        }
        private static void ReportMenu(string systemOp, string report, ref bool endOfReport)
        {
            Console.WriteLine("1.Record to txt file\n2.Exit\n");

            Console.Write($"Number: ");
            string answer = Console.ReadLine();

            switch (answer)
            {
                case "1":
                    Utils.RecordingTxtFile(systemOp, report);
                    break;
                case "2":
                    endOfReport = true;
                    break;
                default:
                    break;
            }
        }
        private static void EngingeSorting(string report,string sortBy)
        {
            bool endOfEngineSorting = false;
            do
            {
                Console.Clear();
                Console.WriteLine($"Sorting by: {sortBy}");
                Console.Write("choose one of this operators { == , != , > , < , >= , <= }: ");
                string answer = Console.ReadLine();

                Func<Product, bool> whichOperator = null;
                switch(answer)
                {
                    case "==":
                        whichOperator = x => GetValue(x, sortBy) == Get
                        break;
                }
            }while(!endOfEngineSorting)
        }
        private static object GetValue(Product product, string propertyName)
        {

        }
        
    }
}

