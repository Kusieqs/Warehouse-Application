using System;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Warehouse_Application
{
    public static class ModificationsAndHistory
    {
        public static void ModifyingProduct(ref List<Product> products, Employee employee)
        {
            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            systemOp = Path.Combine(systemOp, "WareHouse", "Products.json");
            Product copy = null;
            string modifyingRecord;
            string property = string.Empty;
            string value = "";
            int number;
            bool correctNumber, accept;

            bool ListExist = Utils.IsItListEmpty(products);
            if (ListExist)
                return;


            bool correctAnswer = false;
            do
            {
                Utils.GraphicRemovingAndModifying(products, out modifyingRecord, out correctNumber, out number);

                if (correctNumber && number > 0 && number <= products.Count)
                    copy = new Product(products[number - 1]);
                else if (Regex.IsMatch(modifyingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == modifyingRecord))
                    copy = new Product(products.Find(x => x.Id == modifyingRecord));
                else if (number == 0 && correctNumber)
                    return;
                else
                    continue;


                do
                {
                    correctAnswer = true;
                    Console.Clear();
                    copy.ObjectGraphic();
                    Console.WriteLine("7.Exit\n");
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
                            property = "addedBy";
                            break;
                        case "7":
                            property = "x";
                            correctAnswer = false;
                            break;
                        default:
                            continue;
                    }
                } while (string.IsNullOrEmpty(property));

                if (!correctAnswer)
                    continue;

                correctAnswer = false;
                Employee employee1;
                Console.Clear();
                try
                {
                    copy.ObjectGraphic();
                    if (property == "Date")
                    {
                        int daysInMonth;
                        DateTime date;
                        int year, month, day;
                        Console.Write("\n\nYear: ");
                        bool yearBool = int.TryParse(Console.ReadLine(), out year);
                        Console.Write("Month: ");
                        bool monthBool = int.TryParse(Console.ReadLine(), out month);
                        Console.Write("Day: ");
                        bool dayBool = int.TryParse(Console.ReadLine(), out day);

                        if ((yearBool && monthBool && dayBool) && !(year < 1 || month < 1 || month > 12 || day < 1 || year > 2100 || year < 1900))
                            daysInMonth = DateTime.DaysInMonth(year, month);
                        else
                            throw new FormatException("Wrong date");

                        if (daysInMonth >= day)
                        {
                            date = new DateTime(year, month, day);
                            value = date.ToString();
                        }
                        else
                            continue;

                    }
                    else if (property == "addedBy")
                    {
                        string employeeReader = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WareHouse", "Employee.json"));
                        List<Employee> listOfEmployees = JsonConvert.DeserializeObject<List<Employee>>(employeeReader);

                        do
                        {
                            int count = 1;
                            Console.Clear();
                            foreach (var employees in listOfEmployees)
                            {
                                Console.WriteLine($"{count}. {employees.Position} {employees.Name} {employees.LastName}");
                                count++;
                            }
                            Console.Write("\n\nNumber : ");
                            bool itIsCorrect = int.TryParse(Console.ReadLine(), out int numberOfEmployee);

                            if (!itIsCorrect || listOfEmployees.Count < numberOfEmployee || numberOfEmployee <= 0)
                                continue;

                            employee1 = listOfEmployees[numberOfEmployee - 1];

                            value = $"{employee1.Name} {employee1.LastName} {employee1.Position} {employee1.Age} {employee1.Id} {employee1.Login} {employee1.Password} {employee1.LastName} {employee1.mainAccount}";
                            break;

                        } while (true);
                    }
                    else
                    {
                        Console.Write($"\nChanging {property}: ");
                        value = Console.ReadLine();

                        if (string.IsNullOrEmpty(value))
                            throw new FormatException("Lack of infomrations to modify");
                        else if (property == "Price")
                            value = value.Replace('.', ',');
                    }
                    Console.Clear();
                    if (products.Any(x => x.Id == value) && property == "Id")
                        throw new FormatException("This id is already exist");

                    PropertyInfo propertyInfo = copy.GetType().GetProperty(property);
                    object parsedValue = Utils.ParseValue(value, propertyInfo.PropertyType);
                    copy.GetType().GetProperty(property).SetValue(copy, parsedValue);


                    AcceptingModify(copy, out accept);
                    Product jsonBefore = null;

                    if (accept && (correctNumber && number <= products.Count && number > 0))
                    {
                        jsonBefore = new Product(products[number - 1]);
                        products[number - 1].GetType().GetProperty(property).SetValue(products[number - 1], parsedValue);
                        products[number - 1].HistoryOfProduct(new HistoryModifications(new ProductHistory(jsonBefore), new ProductHistory(copy), DateTime.Now, employee, products[number - 1].listOfModifications));
                    }
                    else if (accept && (Regex.IsMatch(modifyingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == modifyingRecord)))
                    {
                        jsonBefore = new Product(products.Find(x => x.Id == modifyingRecord));
                        products.Find(x => x.Id == modifyingRecord).GetType().GetProperty(property).SetValue(products.Find(x => x.Id == modifyingRecord), parsedValue);
                        products.Find(x => x.Id == modifyingRecord).HistoryOfProduct(new HistoryModifications(new ProductHistory(jsonBefore), new ProductHistory(copy), DateTime.Now, employee, products.Find(x => x.Id == modifyingRecord).listOfModifications));
                    }
                    else
                        continue;
                    Program.JsonFileRecord(products);
                    break;
                }
                catch (TargetInvocationException tie)
                {
                    Exception innerException = tie.InnerException;
                    Utils.ExceptionAnswer(innerException.Message);

                }
                catch (FormatException e)
                {
                    Utils.ExceptionAnswer(e.Message);
                }

            } while (true);

        } // Modifying products (id/index)
        public static void ModifyingReportHistory(ref List<Product> listOfProducts, Employee employee)
        {
            bool ListEmpty = Utils.IsItListEmpty(listOfProducts);
            if (ListEmpty)
                return;

            bool endOfModifications = false;
            do
            {
                Product productToChange = new Product();
                int number;
                bool correctNumber;
                string answer;
                Console.Clear();
                int count = 0;
                foreach (var product in listOfProducts)
                {
                    count++;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Number: " + count);
                    Console.ResetColor();
                    Console.WriteLine($"Name:    {product.Name}");
                    Console.WriteLine($"Id:      {product.Id}");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"CHANGES: {product.listOfModifications.Count}");
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine("".PadLeft(40));
                    Console.ResetColor();
                }
                Console.Write("\nWrite number of product or Id (4 Letters and 5 numbers or 0 to exit)\nNumber or id: ");
                answer = Console.ReadLine();
                correctNumber = int.TryParse(answer, out number);
                Console.Clear();

                int index = 0;

                if (answer == "0")
                    break;
                else if (Regex.IsMatch(answer, @"^[A-Za-z]{4}\d{5}$") && listOfProducts.Any(x => x.Id == answer))
                {
                    index = listOfProducts.FindIndex(x => x.Id == answer);
                    productToChange = listOfProducts.Find(x => x.Id == answer);
                }
                else if (correctNumber && number > 0 && number < listOfProducts.Count + 1)
                {
                    index = number - 1;
                    productToChange = listOfProducts[index];
                }
                else
                    continue;

                if (productToChange.listOfModifications.Count <= 0)
                {
                    Console.WriteLine("Lack of modifications\nClick neter to continue");
                    Console.ReadKey();
                }
                else if (productToChange.listOfModifications.Count > 0)
                {
                    do
                    {
                        Console.Clear();
                        foreach (var history in productToChange.listOfModifications)
                        {

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"DATE MODIFICATION: {history.date}\n");
                            Console.WriteLine($"ID MODIFICATION:   {history.idModofication}");
                            Console.WriteLine($"MODIFIED BY:       {history.modifiedBy.Position} {history.modifiedBy.Name} {history.modifiedBy.LastName}\n");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write($"BEFORE\n");
                            Console.ResetColor();
                            Console.Write($"1.Name:{history.before.name}\n2.Price:{history.before.price}\n3.Quantity:{history.before.quantity}\n4.Id:{history.before.id}\n5.Date:{history.before.date}\n6.Added by: {history.before.addedBy.Position} {history.before.addedBy.Name} {history.before.addedBy.LastName}");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("AFTER");
                            Console.ResetColor();
                            Console.Write($"1.Name:{history.after.name}\n2.Price:{history.after.price}\n3.Quantity:{history.after.quantity}\n4.Id:{history.after.id}\n5.Date:{history.after.date}\n6.Added by: {history.after.addedBy.Position} {history.after.addedBy.Name} {history.after.addedBy.LastName}");
                            string x = "".PadLeft(30);
                            Console.WriteLine(x);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.WriteLine();
                            Console.ResetColor();
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nNOW:");
                        Console.ResetColor();
                        productToChange.ObjectGraphic();
                        Console.Write("\n\nWrite a ID of modiciation to undoing modification, 0 to exit or 1 to remove all history\nId: ");
                        string secondAnswer = Console.ReadLine();

                        if (Regex.IsMatch(secondAnswer, @"^[a-zA-Z0-9]{5}$") && productToChange.listOfModifications.Any(x => x.idModofication == secondAnswer))
                        {
                            HistoryModifications h1 = new HistoryModifications();
                            h1 = productToChange.listOfModifications.Find(x => x.idModofication == secondAnswer);
                            productToChange.HistoryOfProduct(new HistoryModifications(new ProductHistory(new Product(productToChange)), new ProductHistory(new Product(h1.before)), DateTime.Now, employee, productToChange.listOfModifications));
                            listOfProducts[index] = new Product(h1.before, productToChange.listOfModifications);
                            break;

                        }
                        else if (secondAnswer == "1")
                        {
                            productToChange.listOfModifications.Clear();
                            listOfProducts[index] = productToChange;
                            break;
                        }
                        else if (secondAnswer == "0")
                        {
                            break;
                        }

                    } while (true);
                }
            } while (!endOfModifications);

        } // undoing modifications/ All history
        private static void AcceptingModify(Product p1, out bool accpet)
        {
            do
            {
                Console.Clear();
                p1.ObjectGraphic();
                Console.Write("\nDo you want to accept this modify?\n1.Yes\n2.No\nNumber: ");
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
            } while (true);

        } // Accepting modifying product
        public static void NewDelivery(ref List<Product> products, Employee employee)
        {
            Product product;
            bool answer = false;
            do
            {
                Console.Clear();
                Console.Write("Write ID or 0 to exit: ");
                string id = Console.ReadLine();

                if (id.Length != 9)
                    continue;
                else if (products.Any(x => x.Id == id))
                {
                    product = new Product(products.Find(x => x.Id == id));
                    int index = products.FindIndex(x => x.Id == id);
                    ModifyingProductDelivery(product, employee, ref products, index);
                    Program.JsonFileRecord(products);
                }
                else if (id == "0")
                    answer = true;
                else if (!products.Any(x => x.Id == id))
                { 
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("This id is not in our database\n\n1. Add product with new ID\n2. Write another Id\n\nNumber: ");
                        string answerId = Console.ReadLine();
                        switch (answerId)
                        {
                            case "1":
                                Utils.AddingProduct(products, employee);
                                Program.JsonFileRecord(products);
                                break;
                            case "2":
                                break;
                            default:
                                continue;
                        }
                        break;
                    } while (true);
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
            bool accept;
            do
            {
                Console.Clear();
                Console.WriteLine("Modifying:\n\n1. Price\n2. Quantity\n3. Exit\n\n");
                copy.ObjectGraphic();
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
            string value = Console.ReadLine();


            PropertyInfo propertyInfo = copy.GetType().GetProperty(property);
            object parsedValue = Utils.ParseValue(value, propertyInfo.PropertyType);
            copy.GetType().GetProperty(property).SetValue(copy, parsedValue);
            Console.Clear();

            AcceptingModify(copy, out accept);
            if (accept)
            {
                copy = new Product(products[index]);
                products[index].GetType().GetProperty(property).SetValue(products[index], parsedValue);
                products[index].HistoryOfProduct(new HistoryModifications(new ProductHistory(product), new ProductHistory(copy), DateTime.Now, employee, products[index].listOfModifications));
            }
        } // modifying engine (Delivery)
    }
}

