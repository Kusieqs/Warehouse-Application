using System;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
namespace Warehouse_Application
{
    public static class ReportMethods
    {
        public static void ReportOfProducts(List<Product> products, string systemOp)
        {
            bool endOfRaport = false;
            if(products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("List is empty\nClick enter to continue");
                Console.ReadKey();
                return;
            }
            do
            {
                Console.Clear();
                Console.WriteLine("REPORTS:");
                Console.Write("1.All Products\n2.Search by id\n3.Sort by values\n4.Exit\n\nNumber: ");
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
                Console.WriteLine("REPORTS\n\n");
                Console.WriteLine("- - - - - - - - - - - -");
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
                Console.Clear();
                List<Product> copyList = products.ToList();
                Console.Write("Id: ");
                string idSearching = Console.ReadLine();
                if (Regex.IsMatch((idSearching), @"^[A-Za-z]{4}\d{5}$"))
                {
                    copyList = copyList.Where(x => x.Id == idSearching).ToList();
                    if (copyList.Count > 0)
                    {
                        Console.Clear();
                        string report = "";
                        foreach (var product in copyList)
                        {
                            report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.date}\n - - - - - - - - \n";
                            Console.WriteLine("Name: " + product.Name);
                            Console.WriteLine("Price: " + product.Price);
                            Console.WriteLine("Quantity: " + product.Quantity);
                            Console.WriteLine("Id: " + product.Id);
                            Console.WriteLine("Date: " + product.date);
                            endSearching = true;
                        }
                        Console.WriteLine("\n\n");
                        ReportMenu(systemOp, report, ref endSearching);

                    }
                    else
                    {
                        Console.WriteLine("\nId is not in the database\nClick enter to continue or 0 to exit");
                        string exitOrNot = Console.ReadLine();
                        if (exitOrNot == "0")
                            endSearching = true;
                    }
                }
                else
                {
                    Console.WriteLine("\nWrong id (4 Letters and 5 numbers, example: Abcd12345)\nClick enter to continue or 0 to exit");
                    string exitOrNot = Console.ReadLine();
                    if (exitOrNot == "0")
                        endSearching = true;
                }

            } while (!endSearching);
        }
        private static void SortingByValue(List<Product> products, string systemOp)
        {
            List<Product> copyList = new List<Product>();
            List<Product> sortList = new List<Product>();
            bool endOfSort = false, attempt = false;
            string sortingBy, operatorSort;
            DateTime dateSorting;
            string value = "";
            int year, month, day;
            do
            {
                do
                {
                    Console.Clear();
                    Console.Write("1.Sort by value price\n2.Sort by value quantity\n3.Sort by date\n4.Exit\n\nNumber: ");
                    sortingBy = Console.ReadLine();
                    switch (sortingBy)
                    {
                        case "1": sortingBy = "Price";
                            attempt = true;
                            break;
                        case "2": sortingBy = "Quantity";
                            attempt = true;
                            break;
                        case "3": sortingBy = "date"; ///.sprobowac sortowania po przez nie date tylko dni albo miesiace albo rok date.Day
                            attempt = true;
                            break;
                        case "4":
                            return;
                        default:
                            break;
                    }

                } while (!attempt);
                attempt = false;
                do
                {
                    Console.Clear();
                    if (sortingBy == "date")
                    {
                        do
                        {
                            bool yearBool = false, monthBool = false, dayBool = false;

                            Console.Clear();
                            Console.Write("Year: ");
                            yearBool = int.TryParse(Console.ReadLine(), out year);
                            Console.Write("Month: ");
                            monthBool = int.TryParse(Console.ReadLine(), out month);
                            Console.Write("Day: ");
                            dayBool = int.TryParse(Console.ReadLine(), out day);
                            if (yearBool && monthBool && dayBool)
                            {
                                if ((year < 1 || month < 1 || month > 12 || day < 1))
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
                                        dateSorting = new DateTime(year,month,day);
                                        value = dateSorting.ToString();
                                        attempt = true;
                                    }
                                }
                            }
                        } while (!attempt);
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write("Value: ");
                        attempt = double.TryParse(Console.ReadLine(), out double x);
                        value = x.ToString();
                        
                    }
                } while (!attempt);
                attempt = false;
                do
                {
                    Console.Clear();
                    Console.Write($"Choose one of this operators ( = , != , > , < , >= , <= )\n\n{sortingBy} [operator] {value}: ");
                    operatorSort = Console.ReadLine();
                    string[] operators = new string[] { "=", "!=", ">", "<", ">=", "<=" };
                    for (int i = 0; i < operators.Length; i++)
                    {
                        if (operatorSort == operators[i])
                            attempt = true;
                    }

                } while (!attempt);

                attempt = false;

                PropertyInfo property1 = typeof(Product).GetProperty(sortingBy);

                if (property1 != null)
                {
                    Func<Product, bool> filter = CreateFilter(property1, operatorSort, value);
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1.Condition to sorted list (All products in sorted list)\n2.Condition to main list (All products)");
                        string answer = Console.ReadLine();
                        switch (answer)
                        {
                            case "1":
                                attempt = true;
                                if (!copyList.Any())
                                {
                                    sortList = products.Where(filter).ToList();
                                }
                                else
                                {
                                    sortList = sortList.Where(filter).ToList();
                                }
                                sortList = sortList.Distinct().ToList();
                                break;
                            case "2":
                                copyList = products.Where(filter).ToList();
                                sortList = sortList.Concat(copyList).ToList();
                                attempt = true;
                                break;
                            default:
                                break;
                        }

                    } while (!attempt);
                    attempt = false;
                }
                bool endOfReport = false;

                do
                {
                    string report = "";
                    if(sortList.Count == 0)
                    {
                        Console.WriteLine("List is empty");
                    }
                    else
                    {
                        Console.Clear();
                        foreach (var product in sortList)
                        {
                            report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.date}\n- - - - - - - - - - -\n";
                            Console.WriteLine($"Name: {product.Name}");
                            Console.WriteLine($"Price: {product.Price}");
                            Console.WriteLine($"Quantity: {product.Quantity}");
                            Console.WriteLine($"Id: {product.Id}");
                            Console.WriteLine($"Date: {product.date}");
                            Console.WriteLine("- - - - - - - - - - - -");
                        }
                    }
                    Console.Write("\n\n1.Report to txt\n2.Antoher term condition\n3.Exit\nNumber: ");
                    string answer = Console.ReadLine();
                    switch(answer)
                    {
                        case "1":
                            ReportMenu(systemOp, report, ref endOfReport);
                            endOfSort = true;
                            break;
                        case "2":
                            endOfReport = true;
                            break;
                        case "3":
                            endOfReport = true;
                            endOfSort = true;
                            break;
                        default:
                            break;
                    }
                } while (!endOfReport);


            } while (!endOfSort); 
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
        private static Func<Product, bool> CreateFilter(PropertyInfo property, string filter, string value)
        {
            var parameter = Expression.Parameter(typeof(Product), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var convertedFilterValue = Expression.Constant(Convert.ChangeType(value, property.PropertyType));
            var comparison = GetComparisonExpression(propertyAccess,filter, convertedFilterValue);
            return Expression.Lambda<Func<Product, bool>>(comparison, parameter).Compile();
        }
        private static Expression GetComparisonExpression(Expression left, string filter, Expression right)
        {
            switch(filter)
            {
                case ">":
                    return Expression.GreaterThan(left, right);
                case "<":
                    return Expression.LessThan(left, right);
                case "=":
                    return Expression.Equal(left, right);
                case "!=":
                    return Expression.NotEqual(left, right);
                case "<=":
                    return Expression.LessThanOrEqual(left, right);
                case ">=":
                    return Expression.GreaterThanOrEqual(left, right);
                default:
                    throw new FormatException("Critical Error");
            }
        }
    }
}

