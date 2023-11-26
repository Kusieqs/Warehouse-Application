using Newtonsoft.Json;
using SixLabors.ImageSharp;
using System.Text.RegularExpressions;

namespace Warehouse_Application
{
    public static class Utils
    {
        public static void FirstTimeUsing(ref List<Product> products, ref string systemOperation, ref List<Employee> employees, ref bool firstTime)
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
        public static void AddingProduct(ref List<Product> products, Employee employee)
        {
            string systemOp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WareHouse", "Products.json");
            bool correctPrice, correctQuantity, correctData = false;
            string name, id, price, quantity;
            DateTime copyDate = DateTime.Now;
            DateTime date = copyDate.Date;
            Product p1 = new Product();
            do
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Write '-' to go to menu\n\n");
                    Console.Write("Name of product: ");
                    name = Console.ReadLine().Trim();
                    if (name == "-")
                        return;

                    p1.Name = name;

                    Console.Write("\nPrice of product: ");
                    price = Console.ReadLine();
                    if (price == "-")
                        return;

                    price = price.Replace('.', ',');
                    correctPrice = double.TryParse(price, out double number);
                    if (!correctPrice)
                        throw new FormatException("Wrong price format");

                    p1.Price = number;

                    Console.Write("\nQuantity of product: ");
                    quantity = Console.ReadLine();
                    if (quantity == "-")
                        return;
                    correctQuantity = int.TryParse(quantity, out int quantityChecked);

                    if (!correctQuantity)
                        throw new FormatException("Wrong quantity format");

                    p1.Quantity = quantityChecked;

                    Console.Write("\nId of product (First 4 letters and 5 numbers, example - AbcD12345): ");
                    id = Console.ReadLine().Trim();
                    if (id == "-")
                        return;

                    p1.Id = id;
                    p1.Date = date;
                    p1.addedBy = employee;

                    string jsonCreator;
                    correctData = true;

                    if (string.IsNullOrEmpty(File.ReadAllText(systemOp)))
                    {
                        List<Product> products1Copy = new List<Product>();
                        products1Copy.Add(p1);
                        jsonCreator = JsonConvert.SerializeObject(products1Copy);
                        File.WriteAllText(systemOp, jsonCreator);
                        break;
                    }
                    else if (!products.Any(x => x.Id == id))
                    {
                        products.Add(p1);
                        jsonCreator = JsonConvert.SerializeObject(products);
                        File.WriteAllText(systemOp, jsonCreator);
                    }
                    else
                        throw new FormatException("Id is already exist!");

                }
                catch (Exception e)
                {
                    correctData = false;
                    ExceptionAnswer(e.Message);
                }
            } while (!correctData);


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nProduct added to list");
            Console.ResetColor();
            Console.WriteLine("Click enter to continue");
            Console.ReadKey();

            string jsonWriter = File.ReadAllText(systemOp);
            products = JsonConvert.DeserializeObject<List<Product>>(jsonWriter);

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
        public static void RemovingRecord(ref List<Product> products)
        {
            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Product p1 = new Product();
            string removingRecord;
            int number;
            bool itIsNumber, correctNumber;
            bool endRemovingRecord = false;
            if (string.IsNullOrEmpty(File.ReadAllText(Path.Combine(systemOp, "WareHouse", "Products.json"))) || products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("List is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            else
            {
                do
                {

                    GraphicRemovingAndModifying(products, out removingRecord, out correctNumber, out number);

                    if (correctNumber && number <= products.Count && number > 0)
                    {
                        p1 = products[number - 1];
                        itIsNumber = true;
                    }
                    else if (Regex.IsMatch(removingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == removingRecord))
                    {
                        p1 = products.Find(x => x.Id == removingRecord);
                        itIsNumber = false;
                    }
                    else if (correctNumber && number == 0)
                    {
                        break;
                    }
                    else
                        continue;

                    Console.Clear();
                    bool choosingCorrect = true;
                    do
                    {
                        choosingCorrect = true;
                        Console.Clear();
                        p1.ObjectGraphic();
                        Console.WriteLine("\nDo you want to remove?\n1.Yes\n2.No");

                        Console.Write("Number: ");
                        string choosingYesNo = Console.ReadLine();

                        if (choosingYesNo == "1")
                        {
                            if (itIsNumber)
                                products.RemoveAt(number - 1);
                            else
                                products.Remove(p1);
                            Program.JsonFileRecord(ref products);

                        }
                        else if (choosingYesNo != "2" && choosingYesNo != "1")
                            choosingCorrect = false;


                    } while (!choosingCorrect);

                } while (!endRemovingRecord);

            }
        } /// removing product from list
        public static string NameFile()
        {
            string x = "";
            bool attempt = false;
            do
            {
                Console.Clear();
                Console.Write("File Name: ");
                x = Console.ReadLine();
                if (x.Length > 0)
                    attempt = true;

            } while (!attempt);
            return x;


        } /// Name of writing down file
        public static void Statistics(List<Product> products)
        {
            string x = "".PadLeft(60);
            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            systemOp = Path.Combine(systemOp, "WareHouse", "Products.json");
            Console.Clear();
            if (string.IsNullOrEmpty(File.ReadAllText(systemOp)))
            {
                Console.Clear();
                Console.WriteLine("List is empty\nClick enter to continue");
                Console.ReadKey();
            }
            else
            {
                double y;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("PRICE");
                Console.ResetColor();
                Console.WriteLine();

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

                y = products.Average(x => x.Price);
                Console.WriteLine($"\nAverage: {y}\n");

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

                y = products.Sum(x => x.Price);
                Console.WriteLine($"\nSum: {y}\n");

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

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

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

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
                            Console.WriteLine($"Name: {p.Name}, ID: {p.Id}, Price: {p.Price}");
                        }
                    }

                }
                Console.WriteLine();
                Console.WriteLine("\n\nClick enter to continue");
                Console.ReadKey();
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("QUANTITY\n");
                Console.ResetColor();

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

                y = products.Average(x => x.Quantity);
                Console.WriteLine($"\nAverage: {y}\n");

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

                y = products.Sum(x => x.Quantity);
                Console.WriteLine($"\nSum: {y}\n");

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

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

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

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

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("DATE\n");
                Console.WriteLine();
                Console.ResetColor();

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

                DateTime d1 = products.Min(x => x.Date);
                var p9 = products.Where(x => x.Date == d1);
                Console.WriteLine("\nThe oldest: \n");
                foreach (var item in p9)
                {
                    Console.WriteLine($"Name: {item.Name}, Id: {item.Id}, Date: {item.Date}");
                }
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

                Console.WriteLine("\nThe newest: \n");
                d1 = products.Max(x => x.Date);
                var p10 = products.Where(x => x.Date == d1);
                foreach (var item in p10)
                {
                    Console.WriteLine($"Name: {item.Name}, Id: {item.Id}, Date: {item.Date}");
                }
                Console.WriteLine();

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(x);
                Console.ResetColor();

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

            }
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
            Console.Write("Write path of file: ");
            string path = Console.ReadLine();
            if (Path.Exists(path) && !string.IsNullOrEmpty(File.ReadAllText(path))
            {
                do
                {
                    try
                    {
                        Console.Clear();
                        string jsonReader = File.ReadAllText(path);
                        List<Product> jsonList = JsonConvert.DeserializeObject<List<Product>>(jsonReader);

                        if (jsonList.Count == 0)
                        {
                            ExceptionAnswer("File is empty");
                            return;
                        }

                        int count = 0;
                        foreach (var item in jsonList)
                        {
                            count++;
                            Console.WriteLine(count);
                            Console.WriteLine(item.ObjectGraphic());
                        }
                        Console.WriteLine("\n\n1.Remove record\n2.Accept list and add to main list\n3.Exit\n\n");
                        Console.Write("Number: ");
                        string answer = Console.ReadLine();
                        bool correctNumber = int.TryParse(answer, out int number);

                        if (!correctNumber)
                            continue;
                        switch (number)
                        {
                            case 1:
                                foreach (var item in jsonList)
                                {
                                    count++;
                                    Console.WriteLine(count);
                                    Console.WriteLine(item.ObjectGraphic());
                                }
                                Console.Write("Write number of record: ");
                                answer = Console.ReadLine();
                                correctNumber = int.TryParse(answer, out number);
                                if (!correctNumber || jsonList.Count < number || number < 1)
                                    continue;
                                jsonList.RemoveAt(number - 1);
                                break;
                            case 2:
                                foreach (var productsFromMainList in products)
                                {
                                    foreach (var addedList in jsonList)
                                    {
                                        if (productsFromMainList.Id == addedList.Id)
                                        {
                                            Console.Clear();
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine($"Conflict with the same ID");
                                            Console.ResetColor();
                                            Console.WriteLine($"1.\n{productsFromMainList.ObjectGraphic()}\n\n\n2.\n{addedList.ObjectGraphic()}");
                                            Console.Write("Choose 1 to remove from main list or 2 to remove from added list (3 to retrun): ");

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
                                        }
                                    }
                                }
                                products = products.Concat(jsonList).ToList();
                            case 3:
                                return;
                            default:
                                continue;
                            }

                        }
                    catch (Exception e)
                    {
                        ExceptionAnswer(e.Message);
                    }
                } while (true)
            }
            else
            {
                ExceptionAnswer("File doesn't exist")
            }
        } /// loading list of products from json file


    }
}

