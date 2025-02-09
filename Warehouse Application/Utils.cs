﻿using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Warehouse_Application
{
    public static class Utils
    {
        public static void FirstTimeUsing(List<Product> products, ref string systemOperation, List<Employee> employees, ref bool firstTime)
        {

            if (!Directory.Exists(systemOperation))
            {
                Directory.CreateDirectory(systemOperation);
                string jsonWriter = string.Empty;
                File.WriteAllText(Path.Combine(systemOperation, "Products.json"), jsonWriter);
                File.WriteAllText(Path.Combine(systemOperation, "Employee.json"), jsonWriter);
                firstTime = true;
            }
            else
                firstTime = false;

            string jsonReader = File.ReadAllText(Path.Combine(systemOperation, "Products.json"));
            products = JsonConvert.DeserializeObject<List<Product>>(jsonReader);

            jsonReader = File.ReadAllText(Path.Combine(systemOperation, "Employee.json"));
            employees = JsonConvert.DeserializeObject<List<Employee>>(jsonReader);
        } /// Checking for existence directory with data
        public static void AddingProduct(List<Product> products, Employee employee,bool delivery = false)
        {
            string systemOp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WareHouse", "Products.json");
            bool correct = false;
            string data;
            do
            {
                try
                {
                    Product p1 = new Product();
                    #region Name
                    Console.Clear();
                    Console.WriteLine("Write '-' to go to menu\n\n");
                    Console.Write("Name of product: ");
                    data = Console.ReadLine().Trim();
                    if (data == "-")
                        return;

                    p1.Name = data;
                    #endregion Name

                    #region Price
                    Console.Write("\nPrice of product: ");
                    data = Console.ReadLine();
                    if (data == "-")
                        return;

                    data = data.Replace(',', '.');
                    correct = double.TryParse(data, out double number);
                    if (!correct)
                        throw new FormatException("Wrong price format");

                    p1.Price = number;
                    #endregion Price

                    #region Quantity
                    Console.Write("\nQuantity of product: ");
                    data = Console.ReadLine();
                    if (data == "-")
                        return;
                    correct = int.TryParse(data, out int quantity);

                    if (!correct)
                        throw new FormatException("Wrong quantity format");

                    p1.Quantity = quantity;

                    #endregion Quantity

                    #region ID
                    Console.Write("\nId of product (First 4 letters and 5 numbers, example - AbcD12345): ");
                    data = Console.ReadLine().Trim();
                    if (data == "-")
                        return;
                    #endregion Id

                    p1.Id = data;
                    p1.Date = DateTime.Now.Date;
                    p1.addedBy = employee;

                    string jsonCreator;
                    if (!products.Any(x => x.Id == data))
                    {
                        products.Add(p1);
                        jsonCreator = JsonConvert.SerializeObject(products);
                        File.WriteAllText(systemOp, jsonCreator);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nProduct added to list");
                        Console.ResetColor();
                        Console.WriteLine("Click enter to continue");
                        Console.ReadKey();
                    }
                    else
                        throw new FormatException("Id is already exist!");

                }
                catch (Exception e)
                {
                    ExceptionAnswer(e.Message);
                }
                if (delivery)
                    break;
            } while (true);

        }/// adding new product to list
        public static void GraphicRemovingAndModifying(List<Product> products, out string answer, out bool correctNumber, out int number)
        {
            Console.Clear();
            int count = 0;
            foreach (var product in products)
            {
                count++;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Number: " + count);
                Console.ResetColor();
                product.ObjectGraphic();
                Console.ResetColor();
            }
            Console.Write("\nWrite number of product or Id (4 Letters and 5 numbers or 0 to exit)\nNumber or id: ");
            answer = Console.ReadLine();
            correctNumber = int.TryParse(answer, out number);
            Console.Clear();

        } ///List of products to see on Removing Method nad Modifying method
        public static void RemovingRecord(List<Product> products)
        {

            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop), removingRecord;
            int number;
            bool correctNumber;
            Product p1 = new Product();

            do
            {
                if (IsItListEmpty(products))
                    return;
                GraphicRemovingAndModifying(products, out removingRecord, out correctNumber, out number);

                if (correctNumber && number <= products.Count && number > 0)
                    p1 = products[number - 1];
                else if (Regex.IsMatch(removingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == removingRecord))
                    p1 = products.Find(x => x.Id == removingRecord);
                else if (correctNumber && number == 0)
                    return;
                else
                    continue;

                Console.Clear();

                do
                {
                    Console.Clear();
                    p1.ObjectGraphic();
                    Console.WriteLine("\nDo you want to remove?\n1.Yes\n2.No");

                    Console.Write("Number: ");
                    string choosingYesNo = Console.ReadLine();

                    if (choosingYesNo == "1")
                    {
                        products.Remove(p1);
                        Program.JsonFileRecord(products);
                        break;
                    }
                    else if (choosingYesNo == "2")
                        break;

                } while (true);

            } while (true);

        }//Removing product from list
        public static string NameFile()
        {
            string x = null;
            bool attempt = false;
            do
            {
                Console.Clear();
                Console.Write("File Name: ");
                x = Console.ReadLine();
                if (x.Length > 0 && Regex.IsMatch(x, @"^[A-Za-z0-9]+$"))
                    attempt = true;

            } while (!attempt);
            return x;


        } /// Name of writing down file
        public static void Statistics(List<Product> products)
        {
            if (IsItListEmpty(products))
                return;

            string x = "".PadLeft(60);
            Console.Clear();

            #region Price Stats
            double y;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("PRICE");
            Console.ResetColor();
            Console.WriteLine();

            WhiteStripe(x);

            y = products.Average(x => x.Price);
            Console.WriteLine($"\nAverage: {y}\n");

            WhiteStripe(x);

            y = products.Sum(x => x.Price);
            Console.WriteLine($"\nSum: {y}\n");

            WhiteStripe(x);

            y = products.Max(x => x.Price);
            var p1 = products.Where(x => x.Price == y).ToList();
            Console.WriteLine("\nMax: ");
            foreach (var item in p1)
            {
                Console.WriteLine($"Name: {item.Name}, Price: {item.Price}, Id: {item.Id}");
            }
            Console.WriteLine("\n");

            y = products.Min(x => x.Price);
            var p2 = products.Where(x => x.Price == y).ToList();
            Console.WriteLine("Min: ");
            foreach (var item in p2)
            {
                Console.WriteLine($"Name: {item.Name}, Price: {item.Price}, Id: {item.Id}");
            }
            Console.WriteLine();

            WhiteStripe(x);

            Console.WriteLine("\nThe most frequently occuring price: \n\n");
            var p3 = products.Select(x => new
            {
                x.Name,
                x.Id,
                x.Price
            }).GroupBy(x => x.Price);

            y = p3.Max(x => x.Count());
            foreach (var item in p3)
            {
                double d = item.Count();
                if (y == d)
                {
                    foreach (var p in item)
                    {
                        Console.WriteLine($"Name: {p.Name}, Price: {p.Price}, Id: {p.Id}");
                    }
                }

            }
            Console.WriteLine();
            Console.WriteLine("\n\nClick enter to continue");
            Console.ReadKey();
            Console.Clear();

            #endregion 

            #region Quantity Stats
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("QUANTITY\n");
            Console.ResetColor();

            WhiteStripe(x);

            y = products.Average(x => x.Quantity);
            Console.WriteLine($"\nAverage: {y}\n");

            WhiteStripe(x);

            y = products.Sum(x => x.Quantity);
            Console.WriteLine($"\nSum: {y}\n");

            WhiteStripe(x);

            y = products.Max(x => x.Quantity);
            var p5 = products.Where(x => x.Quantity == y).ToList();
            Console.WriteLine("\nMax: ");
            foreach (var item in p5)
            {
                Console.WriteLine($"Name: {item.Name}, Quantity: {item.Quantity}, Id: {item.Id}");
            }
            Console.WriteLine("\n");

            y = products.Min(x => x.Quantity);
            var p6 = products.Where(x => x.Quantity == y).ToList();
            Console.WriteLine("Min: ");
            foreach (var item in p6)
            {
                Console.WriteLine($"Name: {item.Name}, Quantity: {item.Quantity}, Id: {item.Id}");
            }
            Console.WriteLine();

            WhiteStripe(x);

            Console.WriteLine("\nThe most frequently occuring quantity: \n\n");
            var p7 = products.Select(x => new
            {
                x.Quantity,
                x.Name,
                x.Id
            }).GroupBy(x => x.Quantity);
            y = p7.Max(x => x.Count());

            foreach (var item in p7)
            {
                double d = item.Count();
                if (d == y)
                {
                    foreach (var p in item)
                    {
                        Console.WriteLine($"Name: {p.Name}, Quantity: {p.Quantity}, Id: {p.Id}");
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("\n\nClick enter to continue");
            Console.ReadKey();
            Console.Clear();
            #endregion

            #region Extra Stats
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("DATE\n");
            Console.WriteLine();
            Console.ResetColor();

            WhiteStripe(x);

            DateTime d1 = products.Min(x => x.Date);
            var p9 = products.Where(x => x.Date == d1);
            Console.WriteLine("\nThe oldest: \n");
            foreach (var item in p9)
            {
                Console.WriteLine($"Name: {item.Name}, Id: {item.Id}, Date: {item.Date}");
            }
            Console.WriteLine();

            WhiteStripe(x);

            Console.WriteLine("\nThe newest: \n");
            d1 = products.Max(x => x.Date);
            var p10 = products.Where(x => x.Date == d1);
            foreach (var item in p10)
            {
                Console.WriteLine($"Name: {item.Name}, Id: {item.Id}, Date: {item.Date}");
            }
            Console.WriteLine();

            WhiteStripe(x);

            Console.WriteLine("\nThe most frequently occuring date: \n\n");
            var p11 = products.Select(x => new
            {
                x.Date,
                x.Name,
                x.Id
            }).GroupBy(x => x.Date);
            y = p11.Max(x => x.Count());
            foreach (var item in p11)
            {
                double d = item.Count();
                if (d == y)
                {
                    foreach (var p in item)
                    {
                        Console.WriteLine($"Name: {p.Name}, Id: {p.Id}, Date: {p.Date}");
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
            Console.WriteLine("\n\nClick enter to continue");
            Console.ReadKey();
            Console.Clear();

            #endregion

        } /// Statistics of products
        public static void ExceptionAnswer(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
            Console.WriteLine("Click enter to continue");
            Console.ReadKey();
        }///message about error
        public static void JsonFileLoad(ref List<Product> products)
        {
            Console.Clear();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Console.Write("Write path of file: ");
            string file = Console.ReadLine() + ".json";
            path = Path.Combine(path, file);
            try
            {
                if (Path.Exists(path))
                {
                    string jsonReader = File.ReadAllText(path);
                    List<Product> jsonList = JsonConvert.DeserializeObject<List<Product>>(jsonReader);
                    if (IsItListEmpty(jsonList))
                        return;
                    do
                    {
                        Console.Clear();

                        int count = 0;
                        foreach (var item in jsonList)
                        {
                            count++;
                            Console.WriteLine(count);
                            item.ObjectGraphic();
                        }
                        count = 0;
                        Console.WriteLine("\n\n1.Remove record\n2.Accept list and add to main list\n3.Exit\n\n");
                        Console.Write("Number: ");
                        string answer = Console.ReadLine();
                        int.TryParse(answer, out int number);

                        if (number == 0)
                            continue;

                        switch (number)
                        {
                            case 1:
                                do
                                {
                                    Console.Clear();
                                    foreach (var item in jsonList)
                                    {
                                        count++;
                                        Console.WriteLine(count);
                                        item.ObjectGraphic();
                                    }
                                    Console.Write("Write number of record: ");
                                    answer = Console.ReadLine();
                                    bool correctNumber = int.TryParse(answer, out number);
                                    if (!correctNumber || jsonList.Count < number || number < 1)
                                        continue;
                                    else
                                        break;

                                } while (true);

                                jsonList.RemoveAt(number - 1);
                                break;

                            case 2:
                                List<Product> jsonProducts = jsonList.ToList();
                                List<Product> mainProducts = products.ToList();
                                foreach (var productsFromMainList in mainProducts)
                                {
                                    foreach (var addedList in jsonProducts)
                                    {
                                        if (productsFromMainList.Id == addedList.Id)
                                        {
                                            Console.Clear();
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine($"Conflict with the same ID");
                                            Console.ResetColor();
                                            Console.WriteLine($"1.\n");
                                            productsFromMainList.ObjectGraphic();
                                            Console.WriteLine("\n2.\n");
                                            addedList.ObjectGraphic();
                                            Console.Write("Choose 1 to remove from main list or 2 to remove from added list (3 to exit): ");

                                            switch (Console.ReadLine())
                                            {
                                                case "1":
                                                    products.Remove(productsFromMainList);
                                                    break;
                                                case "2":
                                                    jsonList.Remove(addedList);
                                                    break;
                                                case "3":
                                                    return;
                                                default:
                                                    continue;
                                            }
                                            Console.Clear();
                                        }
                                    }
                                }
                                products = products.Concat(jsonList).ToList();
                                Program.JsonFileRecord(products);
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("List added to main list");
                                Console.ResetColor();
                                Console.WriteLine("Click enter to continue");
                                Console.ReadKey();
                                return;
                            case 3:
                                return;
                            default:
                                continue;
                        }

                    } while (true);
                }
                else
                    throw new FormatException("File doesn't exist");
            }
            catch (Exception e)
            {
                ExceptionAnswer(e.Message);
            }
        } /// loading list of products from json file
        public static bool IsItListEmpty(List<Product> products)
        {
            if (string.IsNullOrEmpty(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WareHouse", "Products.json"))) || products.Count == 0)
            {
                Console.Clear();
                ExceptionAnswer("List is empty");
                return true;
            }
            else
                return false;
        } /// checking if list is empty
        public static object ParseValue(string input, Type targetType)
        {
            if (targetType == typeof(int) && int.TryParse(input, out int x) && x >= 0)
                return x;
            else if (targetType == typeof(double) && double.TryParse(input, out double y) && y > 0)
                return y;
            else if (targetType == typeof(string) && input.Length > 0)
                return input;
            else if (targetType == typeof(DateTime) && DateTime.TryParse(input, out DateTime date))
                return date;
            else if (targetType == typeof(bool) && bool.TryParse(input, out bool isTrue))
                return isTrue;
            else if (targetType == typeof(Employee))
            {
                string[] employeeInfo = input.Split(' ');
                Enum.TryParse(employeeInfo[2], out PositionName posiation);
                int.TryParse(employeeInfo[3], out int quantity);
                bool.TryParse(employeeInfo[7], out bool main);
                return new Employee(employeeInfo[0], employeeInfo[1], posiation, quantity, employeeInfo[4], employeeInfo[6], employeeInfo[5], main);
            }
            throw new FormatException("Error with target type or value");
        } // Parse value
        private static void WhiteStripe(string x)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(x);
            Console.ResetColor();
        } // split console words with long white stripe

    }
}

