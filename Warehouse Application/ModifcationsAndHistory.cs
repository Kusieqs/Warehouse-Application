using System;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Warehouse_Application
{
    public static class ModificationsAndHistory
    {
        public static void ModifyingProduct(ref List<Product> products,Employee employee)
        {
            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            systemOp = Path.Combine(systemOp, "WareHouse", "Products.json");
            Product copy = null;
            string modifyingRecord;
            string property = string.Empty;
            string value = "";
            int number;
            bool correctNumber, correctModifying = false, accept, graphic = false;

            if (string.IsNullOrEmpty(File.ReadAllText(systemOp)))
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
                        Console.WriteLine($"Date: {copy.Date}");
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
                                property = "Date";
                                break;
                            case "6":
                                return;
                            default:
                                break;
                        }
                    } while (string.IsNullOrEmpty(property));

                    if (property == "Date")
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
                    DateTime d1 = DateTime.Now;
                    PropertyInfo propertyInfo = copy.GetType().GetProperty(property);
                    object parsedValue = ParseValue(value, propertyInfo.PropertyType);
                    copy.GetType().GetProperty(property).SetValue(copy, parsedValue);
                    Console.Clear();
                    if (products.Any(x => x.Id == parsedValue) && property == "Id")
                    {
                        Console.WriteLine("This id is already exist\nClick enter to continue!");
                        Console.ReadKey();
                        continue;
                    }

                    AcceptingModify(copy, out accept);
                    if (accept)
                    {
                        Product jsonBefore = null;
                        if (correctNumber && number <= products.Count && number > 0)
                        {
                            jsonBefore = new Product(products[number - 1]);
                            products[number - 1].GetType().GetProperty(property).SetValue(products[number - 1], parsedValue);
                            products[number - 1].HistoryOfProduct(new HistoryModifications(new ProductHistory(jsonBefore), new ProductHistory(copy), d1, employee, products[number-1].listOfModifications));
                        }
                        else if (Regex.IsMatch(modifyingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == modifyingRecord))
                        {
                            jsonBefore = new Product(products.Find(x => x.Id == modifyingRecord));
                            products.Find(x => x.Id == modifyingRecord).GetType().GetProperty(property).SetValue(products.Find(x => x.Id == modifyingRecord), parsedValue);
                            products.Find(x => x.Id == modifyingRecord).HistoryOfProduct(new HistoryModifications(new ProductHistory(jsonBefore), new ProductHistory(copy), d1, employee,products.Find(x => x.Id == modifyingRecord).listOfModifications));
                        }
                        Program.JsonFileRecord(ref products, systemOp);
                    }
                } while (!correctModifying);
            }
        } // Modifying porducts (id/index)
        public static void ModifyingReportHistory(ref List<Product> listOfProducts, string systemOp, Employee employee)
        {
            if (!File.Exists(systemOp) || string.IsNullOrEmpty(File.ReadAllText(systemOp)))
            {
                Console.Clear();
                Console.WriteLine("Lack of Products with modifications\nClick enter to continue");
                Console.ReadKey();
                return;
            }

            bool endOfModifications = false;
            do
            {
                Product productToChange = new Product();
                int number;
                bool correctNumber, garph = false;
                string answer;
                Console.Clear();
                int count = 0;
                foreach (var product in listOfProducts)
                {
                    count++;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Number: " + count);
                    Console.ResetColor();
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Id: {product.Id}");
                    Console.WriteLine($"CHANGES: {product.listOfModifications.Count}");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine("\n                    \n");
                    Console.ResetColor();
                }
                Console.Write("\nWrite number of product or Id (4 Letters and 5 numbers or 0 to exit)\nNumber or id: ");
                answer = Console.ReadLine();
                correctNumber = int.TryParse(answer, out number);
                Console.Clear();
                int index = 0;
                if (answer == "0")
                {
                    break;
                }
                else if (Regex.IsMatch(answer, @"^[A-Za-z]{4}\d{5}$") && listOfProducts.Any(x => x.Id == answer))
                {
                    index = listOfProducts.FindIndex(x => x.Id == answer);
                    productToChange = listOfProducts.Find(x => x.Id == answer);
                }
                else if (correctNumber && number > 0 && number <= listOfProducts.Count + 1)
                {
                    index = number - 1;
                    productToChange = listOfProducts[index];
                }

                if (productToChange.listOfModifications.Count <= 0)
                {
                    Console.WriteLine("Lack of modifications\nClick neter to continue");
                    Console.ReadKey();
                }
                else if (productToChange.listOfModifications.Count > 0)
                {
                    bool attempt = false;
                    do
                    {
                        Console.Clear();
                        int line = 3;
                        foreach (var history in productToChange.listOfModifications)
                        {

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"DATE MODIFICATION: {history.date}\n");
                            Console.WriteLine($"ID MODIFICATION: {history.idModofication}");
                            Console.ResetColor();
                            Console.Write($"BEFORE\nName:{history.before.name}\nPrice:{history.before.price}\nQuantity:{history.before.quantity}\nId:{history.before.id}\nDate:{history.before.date}");
                            Console.SetCursorPosition(40, line);
                            Console.WriteLine("AFTER");
                            line++;
                            Console.SetCursorPosition(40, line);
                            Console.WriteLine($"Name:{history.after.name}");
                            line++;
                            Console.SetCursorPosition(40, line);
                            Console.WriteLine($"Price:{history.after.price}");
                            line++;
                            Console.SetCursorPosition(40, line);
                            Console.WriteLine($"Quantity:{history.after.quantity}");
                            line++;
                            Console.SetCursorPosition(40, line);
                            Console.WriteLine($"Id:{history.after.id}");
                            line++;
                            Console.SetCursorPosition(40, line);
                            Console.WriteLine($"Date:{history.after.date}\n\n");
                            line += 6;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - \n");
                            Console.ResetColor();
                        }
                        Console.Write("\n\nWrite a ID of modiciation to undoing modification, 0 to exit or 1 to remove all history\nNumber: ");
                        string secondAnswer = Console.ReadLine();

                        if (Regex.IsMatch(secondAnswer, @"^[a-zA-Z0-9]{5}$"))
                        {
                            if (productToChange.listOfModifications.Any(x => x.idModofication == secondAnswer))
                            {
                                DateTime d1 = DateTime.Now;

                                HistoryModifications h1 = new HistoryModifications();
                                h1 = productToChange.listOfModifications.Find(x => x.idModofication == secondAnswer);
                                productToChange.HistoryOfProduct(new HistoryModifications(new ProductHistory(new Product(productToChange)), new ProductHistory(new Product(h1.before)), d1, employee,productToChange.listOfModifications));
                                listOfProducts[index] = new Product(h1.before, productToChange.listOfModifications);
                                attempt = true;
                                Program.JsonFileRecord(ref listOfProducts, systemOp);

                            }
                        }
                        else if (secondAnswer == "1")
                        {
                            productToChange.listOfModifications.Clear();
                            listOfProducts[index] = productToChange;
                            attempt = true;
                        }
                        else if (secondAnswer == "0")
                            attempt = true;

                        Program.JsonFileRecord(ref listOfProducts, systemOp);
                    } while (!attempt);
                }
            } while (!endOfModifications);

        } // undoing modifications/ All history
        public static object ParseValue(string input, Type targetType)
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
        } // Parse value 
        private static void AcceptingModify(Product p1, out bool accpet)
        {
            bool infinity = false;
            do
            {
                Console.WriteLine($"Name: {p1.Name}");
                Console.WriteLine($"Price: {p1.Price}");
                Console.WriteLine($"Quantity: {p1.Quantity}");
                Console.WriteLine($"Id: {p1.Id}");
                Console.WriteLine($"Date: {p1.Date}");
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

        } // Accepting modifying product
        public static void NewDelivery(ref List<Product> products,Employee employee,string systemOp)
        {
            Product product;
            bool answer = false;
            do
            {
                Console.Clear();
                Console.Write("Write ID or 0 to exit: ");
                string id = Console.ReadLine();
                if (products.Any(x => x.Id == id))
                {
                    product = new Product(products.Find(x => x.Id == id));
                    int index = products.FindIndex(x => x.Id == id);
                    ModifyingProductDelivery(product, employee, ref products, index);
                    Program.JsonFileRecord(ref products, systemOp);
                }
                else if (id == "0")
                    answer = true;
                else if (!products.Any(x => x.Id == id))
                {
                    Console.WriteLine("This id is not in our database\nAdd product with new ID\nClick enter to continue\n");
                    Console.ReadKey();
                    Utils.AddingProduct(ref products, systemOp, employee);
                    Program.JsonFileRecord(ref products,systemOp);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wrong id\nClick enter to continue");
                    Console.ReadKey();
                }

            } while (!answer);
        }  // adding new Product to list or changing quantity/ price of product
        private static void ModifyingProductDelivery(Product product, Employee employee, ref List<Product> products, int index)
        {
            Product copy = product;
            string property = string.Empty;
            string value = "";
            int number;
            bool correctNumber, correctModifying = false, accept, graphic = false;


            Console.Clear();
            do
            {
                Console.Clear();
                Console.Write("Modifying:\n1. Price\n2. Quantity\n3. Exit");
                Console.SetCursorPosition(25, 1);
                Console.WriteLine($"Name: {copy.Name}");
                Console.SetCursorPosition(25, 2);
                Console.WriteLine($"Price: {copy.Price}");
                Console.SetCursorPosition(25, 3);
                Console.WriteLine($"Quantity: {copy.Quantity}");
                Console.SetCursorPosition(25, 4);
                Console.WriteLine($"Id: {copy.Id}");
                Console.SetCursorPosition(25, 5);
                Console.WriteLine($"Date: {copy.Date}");
                Console.SetCursorPosition(0, 10);
                Console.Write("Number: ");

                string answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        property = "Price";
                        break;
                    case "2":
                        property = "Quantity";
                        break;
                    case "3":
                        return;
                    default:
                        break;
                }
            } while (string.IsNullOrEmpty(property));

            Console.Write($"Changing {property}: ");
            value = Console.ReadLine();


            DateTime d1 = DateTime.Now;
            PropertyInfo propertyInfo = copy.GetType().GetProperty(property);
            object parsedValue = ParseValue(value, propertyInfo.PropertyType);
            copy.GetType().GetProperty(property).SetValue(copy, parsedValue);
            Console.Clear();

            AcceptingModify(copy, out accept);
            if (accept)
            {
                copy = new Product(products[index]);
                products[index].GetType().GetProperty(property).SetValue(products[index], parsedValue);
                products[index].HistoryOfProduct(new HistoryModifications(new ProductHistory(product), new ProductHistory(copy), d1, employee, products[index].listOfModifications));
            }
        } // modifying engine (Delivery)
    }
}

