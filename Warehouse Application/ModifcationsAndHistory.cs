﻿using System;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Warehouse_Application
{
    public static class ModificationsAndHistory
    {
        public static void ModifyingProduct(ref List<Product> products, string systemOp)
        {
            Product copy = null;
            string modifyingRecord;
            string property = string.Empty;
            string value = "";
            int number;
            bool correctNumber, correctModifying = false, accept, graphic = false;

            if (products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("List is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            else
            {
                Utils.GraphicRemovingAndModifying(products, out modifyingRecord, out correctNumber, out number, ref graphic);
                do
                {

                    if (correctNumber && number > 0 && number <= products.Count)
                    {
                        copy = new Product(products[number - 1]);
                    }
                    else if (Regex.IsMatch(modifyingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == modifyingRecord))
                    {
                        copy = new Product(products.Find(x => x.Id == modifyingRecord));
                    }
                    else if (number == 0 && correctNumber)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Wrong number or id\nClick enter to continue");
                        Console.ReadKey();
                        break;
                    }

                    Console.Clear();
                    do
                    {
                        Console.Clear();
                        Console.Write("Modifying:\n1. Name\n2. Price\n3. Quantity\n4. Id\n5. Date\n6. Exit");
                        Console.SetCursorPosition(25, 1);
                        Console.WriteLine($"Name: {copy.Name}");
                        Console.SetCursorPosition(25, 2);
                        Console.WriteLine($"Price: {copy.Price}");
                        Console.SetCursorPosition(25, 3);
                        Console.WriteLine($"Quantity: {copy.Quantity}");
                        Console.SetCursorPosition(25, 4);
                        Console.WriteLine($"Id: {copy.Id}");
                        Console.SetCursorPosition(25, 5);
                        Console.WriteLine($"Date: {copy.date}");
                        Console.SetCursorPosition(0, 10);
                        Console.Write("Number: ");

                        string answer = Console.ReadLine();
                        switch (answer)
                        {
                            case "1":
                                property = "Name";
                                break;
                            case "2":
                                property = "Price";
                                break;
                            case "3":
                                property = "Quantity";
                                break;
                            case "4":
                                property = "Id";
                                break;
                            case "5":
                                property = "date";
                                break;
                            case "6":
                                return;
                            default:
                                break;
                        }
                    } while (string.IsNullOrEmpty(property));

                    if (property == "date")
                    {
                        DateTime date;
                        int year, month, day;
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
                                Console.WriteLine("Wrong Date\nClick enter to continue");
                                Console.ReadKey();
                                continue;
                            }
                            else
                            {
                                int daysInMonth = DateTime.DaysInMonth(year, month);
                                if (daysInMonth >= day)
                                {
                                    date = new DateTime(year, month, day);
                                    value = date.ToString();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong Date\nClick enter to continue");
                            Console.ReadKey();
                            continue;
                        }
                    }
                    else
                    {
                        Console.Write($"Changing {property}: ");
                        value = Console.ReadLine();
                    }

                    PropertyInfo propertyInfo = copy.GetType().GetProperty(property);
                    object parsedValue = ParseValue(value, propertyInfo.PropertyType);
                    copy.GetType().GetProperty(property).SetValue(copy, parsedValue);
                    Console.Clear();
                    AcceptingModify(copy, out accept);
                    if (accept)
                    {
                        Product jsonBefore = null;
                        if (correctNumber && number <= products.Count && number > 0)
                        {
                            jsonBefore = new Product(products[number - 1]);
                            products[number - 1].GetType().GetProperty(property).SetValue(products[number - 1], parsedValue);

                        }
                        else if (Regex.IsMatch(modifyingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == modifyingRecord))
                        {
                            jsonBefore = new Product(products.Find(x => x.Id == modifyingRecord));
                            products.Find(x => x.Id == modifyingRecord).GetType().GetProperty(property).SetValue(products.Find(x => x.Id == modifyingRecord), parsedValue);
                        }

                        FileOverwriting(jsonBefore, copy);

                        Program.JsonFile(ref products, systemOp);

                    }
                } while (!correctModifying);
            }
        }
        private static void FileOverwriting(Product before, Product after)
        {

            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            systemOp = Path.Combine(systemOp, "Desktop", "History.Json");
            DateTime d1 = DateTime.Now;
            List<HistoryModifications> listOfModifications = new List<HistoryModifications>();

            if (File.Exists(systemOp))
            {
                string jsonReader = File.ReadAllText(systemOp);
                listOfModifications = JsonConvert.DeserializeObject<List<HistoryModifications>>(jsonReader);
            }
            HistoryModifications h1 = new HistoryModifications(before, after, d1);
            listOfModifications.Add(h1);

            string jsonWriter = JsonConvert.SerializeObject(listOfModifications);
            File.WriteAllText(systemOp, jsonWriter);

        }
        public static void ModifyingReportHistory(ref List<Product> listOfProducts)
        {
            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            systemOp = Path.Combine(systemOp, "Desktop", "History.json");

            List<HistoryModifications> historyModifications = new List<HistoryModifications>();

            if (!File.Exists(systemOp) || string.IsNullOrEmpty(File.ReadAllText(systemOp)))
            {
                Console.Clear();
                Console.WriteLine("Lack of modifications\nClick enter to continue");
                Console.ReadKey();
                return;
            }

            bool endModifcations = false;
            HistoryModifications w1 = new HistoryModifications();
            Product p1 = new Product(), p2 = null;

            do
            {
                Console.Clear();
                string reader = File.ReadAllText(systemOp);
                historyModifications = JsonConvert.DeserializeObject<List<HistoryModifications>>(reader);

                int line = 5;
                int count = 1;
                Console.WriteLine("HISTORY");
                Console.WriteLine("- - - - - - - - - - -");
                foreach (var products in historyModifications)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(count);
                    count++;
                    Console.ResetColor();
                    Console.WriteLine($"DATE MODIFICATION: {products.date}\n");
                    Console.Write($"BEFORE\nName:{products.p1.Name}\nPrice:{products.p1.Price}\nQuantity:{products.p1.Quantity}\nId:{products.p1.Id}\nDate{products.p1.date}");
                    Console.SetCursorPosition(40, line);
                    Console.WriteLine("AFTER");
                    line++;
                    Console.SetCursorPosition(40, line);
                    Console.WriteLine($"Name:{products.p2.Name}");
                    line++;
                    Console.SetCursorPosition(40, line);
                    Console.WriteLine($"Price:{products.p2.Price}");
                    line++;
                    Console.SetCursorPosition(40, line);
                    Console.WriteLine($"Quantity:{products.p2.Quantity}");
                    line++;
                    Console.SetCursorPosition(40, line);
                    Console.WriteLine($"Id:{products.p2.Id}");
                    line++;
                    Console.SetCursorPosition(40, line);
                    Console.WriteLine($"Date:{products.p2.date}");
                    line++;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(" - - - - - - ");
                    Console.ResetColor();
                    line += 4;
                }
                Console.Write("\n\nWrite 0 to exit or Id/Number of product to undoing modifications\nNumber: ");
                string answer = Console.ReadLine();
                bool correctNumber = int.TryParse(answer, out int index);

                if (answer == "0")
                {
                    return;
                }
                else if (Regex.IsMatch(answer, @"^[A-Za-z]{4}\d{5}$") && listOfProducts.Any(x => x.Id == answer))
                {
                    w1 = historyModifications.Find(x => x.p2.Id == answer);


                    int findIndex = listOfProducts.FindIndex(x => x.Id == w1.p2.Id);

                    p1 = listOfProducts[findIndex];


                    listOfProducts[findIndex] = w1.p1;


                    FileOverwriting(p1, listOfProducts[findIndex]);
                }
                else if (correctNumber && index > 0 && index <= historyModifications.Count+1)
                {
                    w1 = historyModifications[index - 1];

                    int findIndex = listOfProducts.FindIndex(x => x == w1.p2);

                    p1 = listOfProducts[findIndex];

                    listOfProducts[findIndex] = w1.p1;

                    FileOverwriting(p1, listOfProducts[findIndex]);
                }

            } while (!endModifcations);

        }
        private static object ParseValue(string input, Type targetType)
        {
            if (targetType == typeof(int))
            {
                if (int.TryParse(input, out int x))
                {
                    return x;
                }
            }
            else if (targetType == typeof(double))
            {
                if (double.TryParse(input, out double x))
                {
                    return x;
                }
            }
            else if (targetType == typeof(string))
            {
                return input;
            }
            else if (targetType == typeof(DateTime))
            {
                if (DateTime.TryParse(input, out DateTime x))
                {
                    return x;
                }
            }
            throw new FormatException("Error with target type");
        }
        private static void AcceptingModify(Product p1, out bool accpet)
        {
            bool infinity = false;
            do
            {
                Console.WriteLine($"Name: {p1.Name}");
                Console.WriteLine($"Price: {p1.Price}");
                Console.WriteLine($"Quantity: {p1.Quantity}");
                Console.WriteLine($"Id: {p1.Id}");
                Console.WriteLine($"Date: {p1.date}");
                Console.WriteLine("\nDo you want to accept this modify?\n1.Yes\n2.No");
                string answer = Console.ReadLine();
                if (answer == "1")
                {
                    accpet = true;
                    return;
                }
                else if (answer == "2")
                {
                    accpet = false;
                    return;
                }
            } while (!infinity);
            accpet = false;

        }
    }
}

