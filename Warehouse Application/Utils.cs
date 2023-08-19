using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Warehouse_Application
{
    public static class Utils
    {
        public static void FirstTimeUsing(ref List<Product> products, ref string systemOperation, ref List<Employee> employees,ref bool firstTime)
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
        }
        public static void AddingProduct(ref List<Product> products, string systemOp, Employee employee)
        {

            bool correctPrice, correctQuantity, correctData = false;
            string name, id;
            int quantity;
            double price;
            DateTime copyDate = DateTime.Now;
            DateTime date = copyDate.Date;


            do
            {
                Console.Clear();
                Console.Write("Name of product: ");
                name = Console.ReadLine().Trim();

                Console.Write("\nPrice of product: ");
                correctPrice = double.TryParse(Console.ReadLine(), out price);

                Console.Write("\nQuantity of product: ");
                correctQuantity = int.TryParse(Console.ReadLine(), out quantity);

                Console.Write("\nId of product (First 4 letters and 5 numbers, example - AbcD12345): ");
                id = Console.ReadLine().Trim();


                try
                {

                    if (correctPrice && correctQuantity)
                    {
                        Product p1 = new Product(name, id, price, quantity, date, employee);

                        string jsonCreator;
                        correctData = false;
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
                            correctData = true;

                        }
                        else
                            throw new FormatException("Id is already exist!");
                    }
                    else
                    {
                        throw new FormatException("Wrong data or Id already exist");
                    }
                }
                catch (FormatException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n{e.Message}");
                    Console.ResetColor();
                    Console.WriteLine("Click enter to continue or 0 to exit");
                    string answer = Console.ReadLine();
                    if (answer == "0")
                        return;
                }

            } while (!correctData);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nProduct added to list");
            Console.ResetColor();
            Console.WriteLine("Click enter to continue");
            Console.ReadKey();

            string jsonWriter = File.ReadAllText(systemOp);
            products = JsonConvert.DeserializeObject<List<Product>>(jsonWriter);

        }
        public static void RecordingTxtFile(string systemOp, string report)
        {
            if (!string.IsNullOrEmpty(systemOp))
            {
                Console.Clear();
                Console.Write("File Name: ");
                string fileName = Console.ReadLine() + ".txt";
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                path = Path.Combine(path, fileName);
                File.WriteAllText(path, report);
                Console.WriteLine("File is complete!");
            }
            else
            {
                Console.WriteLine("File is empty!\nClick enter to continue");
                Console.ReadKey();
            }
        }
        public static void GraphicRemovingAndModifying(List<Product> products, out string answer, out bool correctNumber, out int number, ref bool graphic)
        {
            graphic = true;
            Console.Clear();
            int count = 0;
            foreach (var product in products)
            {
                count++;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Number: " + count);
                Console.ResetColor();
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: {product.Price}");
                Console.WriteLine($"Quantity: {product.Quantity}");
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Date: {product.Date}\n\n");
                Console.ResetColor();
            }
            Console.Write("\nWrite number of product or Id (4 Letters and 5 numbers or 0 to exit)\nNumber or id: ");
            answer = Console.ReadLine();
            correctNumber = int.TryParse(answer, out number);
            Console.Clear();

        }
        public static void RemovingRecord(ref List<Product> products, string systemOp)
        {
            Product p1 = new Product();
            string removingRecord;
            int number;
            bool itIsNumber, correctNumber, graphic = false;
            bool endRemovingRecord = false;
            if (products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("List is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            else
            {
                do
                {
                    GraphicRemovingAndModifying(products, out removingRecord, out correctNumber, out number, ref graphic);

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
                    {
                        Console.WriteLine("Wrong number or id\nClick enter to continue");
                        Console.ReadKey();
                        continue;
                    }
                    Console.Clear();
                    bool choosingCorrect = false;
                    do
                    {
                        Console.WriteLine($"Name: {p1.Name}");
                        Console.WriteLine($"Price: {p1.Price}");
                        Console.WriteLine($"Quantity: {p1.Quantity}");
                        Console.WriteLine($"Id: {p1.Id}");
                        Console.WriteLine($"Date: {p1.Date}");
                        Console.WriteLine("\nDo you want to remove?\n1.Yes\n2.No");
                        Console.Write("Number: ");
                        string choosingYesNo = Console.ReadLine();

                        if (choosingYesNo == "1")
                        {
                            choosingCorrect = true;
                            if (itIsNumber)
                                products.RemoveAt(number - 1);
                            else
                                products.Remove(p1);
                            Program.JsonFileRecord(ref products, systemOp);

                        }
                        else if (choosingYesNo == "2")
                            choosingCorrect = true;


                    } while (!choosingCorrect);

                } while (!endRemovingRecord);

            }



        }
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


        }
        public static void Statistics(List<Product> products)
        {
            Console.Clear();
            if (products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("List is empty\nClick enter to continue");
                Console.ReadKey();
            }
            else
            {
                double y;
                Console.WriteLine("PRICE");
                Console.WriteLine();

                y = products.Average(x => x.Price);
                Console.WriteLine($"Average: {y}\n");
                Console.WriteLine();

                y = products.Sum(x => x.Price);
                Console.WriteLine($"Sum: {y}\n");
                Console.WriteLine();

                y = products.Max(x => x.Price);
                var p1 = products.Where(x => x.Price == y).ToList();
                Console.WriteLine("Max: ");
                foreach (var item in p1)
                {
                    Console.WriteLine($"Name: {item.Name}, Price: {item.Price}, Id: {item.Id}");
                }
                Console.WriteLine();
                Console.WriteLine();
                y = products.Min(x => x.Price);
                var p2 = products.Where(x => x.Price == y).ToList();
                Console.WriteLine("Min: ");
                foreach (var item in p2)
                {
                    Console.WriteLine($"Name: {item.Name}, Price: {item.Price}, Id: {item.Id}");
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("The most frequently occuring price: \n");
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
                        Console.WriteLine();
                    }

                }
                Console.WriteLine();
                Console.WriteLine("\n\nClick enter to continue");
                Console.ReadKey();
                Console.Clear();

                Console.WriteLine("QUANTITY\n\n");
                y = products.Average(x => x.Quantity);
                Console.WriteLine($"Average: {y}\n\n");
                y = products.Sum(x => x.Quantity);
                Console.WriteLine($"Sum: {y}\n\n");
                y = products.Max(x => x.Quantity);
                var p5 = products.Where(x => x.Quantity == y).ToList();
                Console.WriteLine("Max: ");
                foreach (var item in p5)
                {
                    Console.WriteLine($"Name: {item.Name}, Quantity: {item.Quantity}, Id: {item.Id}");
                }
                Console.WriteLine();
                Console.WriteLine();
                y = products.Min(x => x.Quantity);
                var p6 = products.Where(x => x.Quantity == y).ToList();
                Console.WriteLine("Min: ");
                foreach (var item in p6)
                {
                    Console.WriteLine($"Name: {item.Name}, Quantity: {item.Quantity}, Id: {item.Id}");
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("The most frequently occuring quantity: \n");

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
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
                Console.WriteLine("\n\nClick enter to continue");
                Console.ReadKey();
                Console.Clear();

                Console.WriteLine("DATE\n\n");
                Console.WriteLine();

                DateTime d1 = products.Min(x => x.Date);
                var p9 = products.Where(x => x.Date == d1);
                Console.WriteLine("The oldest: \n");
                foreach (var item in p9)
                {
                    Console.WriteLine($"Name: {item.Name}, Id: {item.Id}, Date: {item.Date}");
                }

                Console.WriteLine();
                Console.WriteLine("\n\nThe newest: \n");
                d1 = products.Max(x => x.Date);
                var p10 = products.Where(x => x.Date == d1);
                foreach (var item in p10)
                {
                    Console.WriteLine($"Name: {item.Name}, Id: {item.Id}, Date: {item.Date}");
                }
                Console.WriteLine();
                Console.WriteLine("\n\nThe most frequently occuring date: \n");
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
        }
    }
}

