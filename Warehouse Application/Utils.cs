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
        private static void GraphicRemovingAndModifying(List<Product> products, out string answer, out bool correctNumber, out int number)
        {
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
            Product p1 = new Product();
            string modifyingRecord;
            int number;
            bool itIsNumber, correctNumber, correctModifying = false, accept;
            bool endRemovingRecord = false;

            if (products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("List is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            else
            {
                string property = "";
                do
                {
                    GraphicRemovingAndModifying(products, out modifyingRecord, out correctNumber, out number);

                    if (correctNumber && number <= products.Count && number > 0)
                    {
                        do
                        {
                            Console.Clear();
                            Console.Write("Modifying:\n1. Name\n2. Price\n3. Quantity\n4. Id\n5. Date\n6. Exit \nNumber: ");
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
                                    continue;
                            }
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
                                        int daysInMonth = DateTime.DaysInMonth(year, month);
                                        Console.WriteLine("Wrong Date\nClick enter to continue");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        int daysInMonth = DateTime.DaysInMonth(year, month);
                                        if (daysInMonth >= day)
                                        {
                                            date = new DateTime(year, month, day);
                                            p1 = new Product(products[number - 1].Name, products[number - 1].Id, products[number - 1].Price, products[number - 1].Quantity, products[number - 1].date);
                                            p1.GetType().GetProperty(property).SetValue(p1, date);

                                            AcceptingModify(p1,out accept);
                                            if(accept)
                                            {
                                                products[number - 1].GetType().GetProperty(property).SetValue(products[number - 1], date);
                                                correctModifying = true;

                                            }
                                        }
                                    }
                                }
                            }
                            else 
                            {
                                string value = null;
                                Console.Write($"Changing {property}: ");
                                switch(property)
                                {
                                    case "Name":
                                        value = Console.ReadLine();
                                        break;
                                    case "Price":
                                        bool coorectPrice = double.TryParse(Console.ReadLine(), out value); ///?????
                                        break;
                                    /// zrobic try catch
                                }


                                p1 = new Product(products[number - 1].Name, products[number - 1].Id, products[number - 1].Price, products[number - 1].Quantity, products[number - 1].date);
                                p1.GetType().GetProperty(property).SetValue(p1, value);
                                AcceptingModify(p1,out accept);
                                if(accept)
                                {
                                    products[number - 1].GetType().GetProperty(property).SetValue(products[number - 1], value);
                                    correctModifying = true;
                                }
                            }

                        } while (!correctModifying);

                    }
                    else if (Regex.IsMatch(modifyingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == modifyingRecord))
                    {
                        p1 = products.Find(x => x.Id == modifyingRecord);
                        itIsNumber = false;
                    }
                    else if(number == 0 && correctNumber)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Wrong number or id\nClick enter to continue");
                        Console.ReadKey();
                        continue;
                    }
                            Console.Clear();

                } while (!endRemovingRecord);

                        /////



            }
        }
        public static void RemovingRecord(ref List<Product> products, string systemOp)
            {
                Product p1 = new Product();
                string removingRecord;
                int number;
                bool itIsNumber, correctNumber;
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
        
    }
}
