using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Reflection;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Warehouse_Application
{
    public static class Utils
    {
        public static void FirstTimeUsing(ref List<Product> products, ref string systemOperation)
        {
            if (File.Exists(systemOperation) && string.IsNullOrEmpty(File.ReadAllText(Path.Combine(systemOperation, "Products.json"))))
            {
                try
                {
                    File.Delete(systemOperation);
                }
                catch (IOException e)
                {
                    Console.WriteLine("Wystapil blad podczas usówania pliku: " + e);
                }
            }

            systemOperation = Path.Combine(systemOperation, "Products.json");

            if (!File.Exists(systemOperation))
            {
                string jsonWriter = string.Empty;
                File.WriteAllText(systemOperation, jsonWriter);
            }
            else
            {
                string jsonReader = File.ReadAllText(systemOperation);
                products = JsonConvert.DeserializeObject<List<Product>>(jsonReader);
            }
        }
        public static void AddingProduct(ref List<Product> products, string systemOp)
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
                    Product p1 = new Product(name, id, price, quantity, date);

                    string jsonCreator;
                    correctData = true;
                    if (string.IsNullOrEmpty(File.ReadAllText(systemOp)))
                    {
                        List<Product> products1Copy = new List<Product>();
                        products1Copy.Add(p1);
                        jsonCreator = JsonConvert.SerializeObject(products1Copy);
                        File.WriteAllText(systemOp, jsonCreator);
                    }
                    else
                    {
                        products.Add(p1);
                        jsonCreator = JsonConvert.SerializeObject(products);
                        File.WriteAllText(systemOp, jsonCreator);

                    }
                    correctData = true;
                }
                catch (FormatException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n{e.Message}");
                    Console.ResetColor();
                    Console.WriteLine("Click enter to continue");
                    Console.ReadKey();
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
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                path = Path.Combine(path, "Desktop", fileName);
                File.WriteAllText(path, report);
                Console.WriteLine("File is complete!");
            }
            else
            {
                Console.WriteLine("File is empty!\nClick enter to continue");
                Console.ReadKey();
            }
        }
        private static void GraphicRemovingAndModifying(List<Product> products, out string answer, out bool correctNumber, out int number, ref bool graphic)
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
                Console.WriteLine($"Date: {product.date}");
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("\n                    \n");
                Console.ResetColor();
            }
            Console.Write("\nWrite number of product or Id (4 Letters and 5 numbers or 0 to exit)\nNumber or id: ");
            answer = Console.ReadLine();
            correctNumber = int.TryParse(answer, out number);
            Console.Clear();

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
                if(answer == "1")
                {
                    accpet = true;
                    return;
                }
                else if(answer == "2")
                {
                    accpet = false;
                    return;
                }
            } while (!infinity);
            accpet = false;

        }
        public static void ModifyingProduct(ref List<Product> products)
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

                do
                {
                    GraphicRemovingAndModifying(products, out modifyingRecord, out correctNumber, out number,ref graphic);
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
                        Console.Clear(); ////
                        AcceptingModify(copy, out accept);
                        if(accept)
                        {
                            if (correctNumber && number <= products.Count && number > 0)
                            {
                                products[number - 1].GetType().GetProperty(property).SetValue(products[number - 1], parsedValue);

                            }
                            else if (Regex.IsMatch(modifyingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == modifyingRecord))
                            {
                                products.Find(x => x.Id == modifyingRecord).GetType().GetProperty(property).SetValue(products.Find(x => x.Id == modifyingRecord), parsedValue);
                            }
                        }
                    } while (!correctModifying);
                } while (!correctModifying);
            }
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
                            Console.WriteLine($"Date: {p1.date}");
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

                                string jsonCreator = JsonConvert.SerializeObject(products);
                                File.WriteAllText(systemOp, jsonCreator);

                                string jsonWriter = File.ReadAllText(systemOp);
                                products = JsonConvert.DeserializeObject<List<Product>>(jsonWriter);

                            }
                            else if (choosingYesNo == "2")
                                choosingCorrect = true;


                        } while (!choosingCorrect);

                    } while (!endRemovingRecord);

                }
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
                if(double.TryParse(input, out double x))
                {
                    return x;
                }
            }
            else if (targetType == typeof(string))
            {
                return input;
            }
            else if(targetType == typeof(DateTime))
            {
                if(DateTime.TryParse(input, out DateTime x))
                {
                    return x;
                }
            }
            throw new FormatException("Error with target type");
        }
        
    }
}
