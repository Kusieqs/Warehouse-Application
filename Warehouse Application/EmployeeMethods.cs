using System;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Warehouse_Application
{
    public static class EmployeeMethods
    {
        private static void AcceptingModify(Employee e1, out bool accpet)
        {
            do
            {
                e1.GraphicEmployee();
                Console.WriteLine("\n\nDo you want to accept this modify?\n1.Yes\n2.No");
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

        } /// Accepting Modifying employee
        public static void AddingEmployee(ref List<Employee> employees, bool firsTime)
        {
            Employee employee = new Employee();
            do
            {
                try
                {
                    Console.Clear();
                    Console.Write("Choose position of employee\n\n1.Admin\n2.Supplier\n3.Manager\n4.Employee\n0.Exit\n\nNumber: ");
                    string position = Console.ReadLine();
                    switch (position)
                    {
                        case "1":
                            employee.Position = PositionName.Admin;
                            break;
                        case "2":
                            employee.Position = PositionName.Supplier;
                            break;
                        case "3":
                            employee.Position = PositionName.Manager;
                            break;
                        case "4":
                            employee.Position = PositionName.Employee;
                            break;
                        case "0":
                            return;
                        default:
                            continue;
                    }

                    Console.Clear();
                    Console.WriteLine($"Position: {employee.Position}");

                    NewEmployeeInformation(employee, ref employees, firsTime);

                    break;

                }
                catch (FormatException e)
                {
                    Utils.ExceptionAnswer(e.Message);
                }
            } while (true);


        } /// adding new employee to list
        public static void ChoosingEmployee(ref List<Employee> employees, ref Employee employee, bool firstTime)
        {
            do
            {
                try
                {
                    Console.Clear();
                    if (string.IsNullOrEmpty(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WareHouse", "Employee.json"))))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("Welcome to warehouse app. Please provide information for the main admin");
                        Console.ResetColor();

                        Console.WriteLine("\n\nAdmin Information");
                        employee.Position = PositionName.Admin;

                        NewEmployeeInformation(employee,ref employees, firstTime);
                    }

                    int line = 1;
                    Console.Clear();
                    foreach (var worker in employees)
                    {
                        Console.WriteLine($"{line}. {worker.Name} {worker.LastName} {worker.Position}");
                        ++line;
                    }
                    Console.WriteLine("0. Exit");
                    Console.Write("\nNumber: ");
                    bool correctNumber = int.TryParse(Console.ReadLine(), out int number);

                    if (!correctNumber || employees.Count < number)
                        continue;
                    else if (number == 0)
                        Environment.Exit(0);

                    employee = employees[number - 1];

                    Console.Clear();
                    Console.WriteLine("LOGIN");
                    Console.Write("\n\nLogin: ");
                    string login = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = Console.ReadLine();

                    if (employee.Login == login && employee.Password != password)
                        throw new FormatException("Wrong password");
                    else if (employee.Login != login && employee.Password == password)
                        throw new FormatException("Wrong login");
                    else if (employee.Login == login && employee.Password == password)
                        break;
                    else
                        throw new FormatException("Wrong login and password");
                }
                catch (Exception e)
                {
                    Utils.ExceptionAnswer(e.Message);
                }
            } while (true);
        }/// Setting first admin or choosing employee
        public static void MenuOfEmployee(ref List<Employee> listEmployees)
        {
            do
            {
                try
                {
                    Console.Clear();
                    Console.Write("1.Remove\n2.Modifying\n3.Exit\n\nNumber: ");
                    string answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "1":
                            RemovingEmployee(ref listEmployees);
                            break;
                        case "2":
                            EmployeeModifying(ref listEmployees);
                            break;
                        case "3":
                            return;
                        default:
                            continue;
                    }
                }
                catch (TargetInvocationException tie)
                {
                    Exception innerException = tie.InnerException;

                    Utils.ExceptionAnswer(innerException.Message);

                }
                catch (Exception ex)
                {
                    Utils.ExceptionAnswer(ex.Message);
                }
                Console.Clear();
                break;
            } while (true);
        } ///Options to remove or modifying employee
        private static void NewEmployeeInformation(Employee employee, ref List<Employee> employees, bool firsTime)
        {
            Console.Write($"Name: ");
            string name = Console.ReadLine();
            if (Regex.IsMatch(name, @"^[A-Za-z]+$"))
            {
                name = name.ToLower();
                name = char.ToUpper(name[0]) + name.Substring(1);
                employee.Name = name;
            }
            else
                throw new FormatException("Wrong name format");


            Console.Write("Last name: ");
            string lastName = Console.ReadLine();
            if (Regex.IsMatch(lastName, @"^[A-Za-z]+$"))
            {
                lastName = lastName.ToLower();
                lastName = char.ToUpper(lastName[0]) + lastName.Substring(1);
                employee.LastName = lastName;
            }
            else
                throw new FormatException("Wrong last name format");
            
            Console.Write("Id (3 chars): ");
            string id = Console.ReadLine();

            if(Regex.IsMatch(id, @"^[A-Za-z0-9]+$") && !employees.Any(x => x.Id == id))
                employee.Id = id;
            else if(employees.Any(x => x.Id == id))
                throw new FormatException("This id is already exist");
            else
                throw new FormatException("Wrong id format");


            Console.Write("Age: ");
            bool x = int.TryParse(Console.ReadLine(), out int age);
            if (!x)
                throw new FormatException("Wrong age");
            employee.Age = age;

            string login = name.Substring(0, 3);

            do
            {
                for (int i = 0; i < 3; i++)
                {
                    Random random = new Random();
                    int p = random.Next(0, 10);
                    login += p.ToString();
                }
                if (firsTime || string.IsNullOrEmpty(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WareHouse", "Employee.json"))))
                    break;
                else if (!employees.Any(x => x.Login == login))
                    break;

            } while (true);

            Console.WriteLine($"\n\nLogin: {login}");
            employee.Login = login;

            Console.Write("\nPassword (min 6 characters): ");
            string password = Console.ReadLine();
            employee.Password = password;

            if (firsTime)
                employee.mainAccount = true;
            else
                employee.mainAccount = false;

            string json;
            List<Employee> firsTimeList = new List<Employee>();

            Console.Clear();
            Console.WriteLine($"WRITE DOWN THIS INFORMATION:\n\nLogin: {employee.Login}\nPassword: {employee.Password}\n\n\nClick enter to continue");
            Console.ReadKey();

            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (firsTime)
            {
                firsTimeList.Add(employee);
                json = JsonConvert.SerializeObject(firsTimeList);
            }
            else if(string.IsNullOrEmpty(File.ReadAllText(Path.Combine(systemOp, "WareHouse", "Employee.json"))))
            {
                List<Employee> employees1 = new List<Employee>();
                employees1.Add(employee);
                json = JsonConvert.SerializeObject(employees1);
            }
            else
            {
                employees.Add(employee);
                json = JsonConvert.SerializeObject(employees);
            }

            File.WriteAllText(Path.Combine(systemOp, "WareHouse", "Employee.json"), json);

            string jsonReader = File.ReadAllText(Path.Combine(systemOp, "WareHouse", "Employee.json"));
            employees = JsonConvert.DeserializeObject<List<Employee>>(jsonReader);

        } /// Adding informations do new Employee 
        private static void EmployeeModifyingData(ref Employee employee, List<Employee> employees)
        {
            Employee copy = employee;
            string property = "", value = "";

            do
            {
                Console.Clear();
                employee.GraphicEmployee();
                Console.WriteLine("9.Exit");
                Console.Write("Number: ");

                string answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        property = "Name";
                        break;
                    case "2":
                        property = "LastName";
                        break;
                    case "3":
                        property = "Id";
                        break;
                    case "4":
                        property = "Age";
                        break;
                    case "5":
                        property = "Position";
                        break;
                    case "6":
                        property = "Password";
                        break;
                    case "7":
                        property = "Login";
                        break;
                    case "8":
                        property = "mainAccount";
                        break;
                    case "9":
                        return;
                    default:
                        continue;
                        
                }
                break;
            } while (true);


            if (property == "Position")
            {
                do
                {
                    
                    Console.Clear();
                    Console.Write("1.Admin\n2.Supplier\n3.Employee\n4.Manager\n5.Exit\nNumber: ");
                    string answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "1":
                            value = PositionName.Admin.ToString();
                            break;
                        case "2":
                            value = PositionName.Supplier.ToString();
                            break;
                        case "3":
                            value = PositionName.Employee.ToString();
                            break;
                        case "4":
                            value = PositionName.Manager.ToString();
                            break;
                        case "5":
                            return;
                        default:
                            continue;
                            

                    }
                    break;
                } while (true);
            }
            else if (property == "mainAccount")
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("1.Main account (true)\n2.Main account (false)\n3.Exit");
                    string answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "1":
                            value = true.ToString();
                            break;
                        case "2":
                            value = false.ToString();
                            break;
                        case "3":
                            return;
                        default:
                            continue;
                    }
                    break;
                } while (true);
            }
            else
            {
                Console.Write($"Changing {property}: ");
                value = Console.ReadLine();
            }

            bool accept;


            PropertyInfo propertyInfo = copy.GetType().GetProperty(property);
            object valueParsed = Utils.ParseValue(value, propertyInfo.PropertyType);
            copy.GetType().GetProperty(property).SetValue(copy, valueParsed);

            if (employees.Any(x => x.Id == valueParsed) && property == "Id")
                throw new FormatException("This id is already exist");

            Console.Clear();
            AcceptingModify(copy, out accept);

            if (accept)
                employee = copy;

        } // Changing informations about employee
        private static void RemovingEmployee(ref List<Employee> listEmployees)
        {
            bool correctNumber = false;

            do
            {
                Console.Clear();
                int count = 0;
                foreach (var employee in listEmployees)
                {
                    ++count;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{count}.");
                    Console.ResetColor();
                    employee.GraphicEmployee();
                }
                Console.Write("\n\nNumber of employee (0 to exit): ");
                correctNumber = int.TryParse(Console.ReadLine(), out int number);

                if (correctNumber && number == 0)
                    break;
                else if ((correctNumber && number > 0 && number <= count)&&(!listEmployees[number - 1].mainAccount && listEmployees.Count > 1))
                {
                    Console.Clear();
                    Console.WriteLine("Are you sure to delete this account? \n1.Yes\n2.No");
                    int.TryParse(Console.ReadLine(), out int result);
                    if (result == 1)
                    {
                        listEmployees.RemoveAt(number - 1);
                        string system = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string json = JsonConvert.SerializeObject(listEmployees);
                        File.WriteAllText(system, json);
                    }
                    else
                        continue;
                }
                else if ((correctNumber && number > 0 && number <= count) && (listEmployees.Count == 1 || listEmployees.Count(x => x.Position == PositionName.Admin) == 1))
                {
                    do
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("WARNING\n\n");
                        Console.ResetColor();
                        Console.Write("You are going to delete last admin position\nThis will delete all files\n\n1.Delete\n2.Continue\n\nNumber: ");
                        int.TryParse(Console.ReadLine(), out int result);

                        if (result == 1)
                        {
                            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            Directory.Delete(Path.Combine(systemOp, "WareHouse"), true);
                            Environment.Exit(0); 
                        }
                        else if (result == 2)
                            break;

                    } while (true);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Its main Admin! You have to change priority of this account.\nClick enter to continue!");
                    Console.ReadKey();
                }

            } while (true);

        }// Deleting employee
        public static void EmployeeModifying(ref List<Employee> listEmployees)
        {
            do
            {
                Console.Clear();
                int count = 0;
                foreach (var employee in listEmployees)
                {
                    ++count;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{count}.");
                    Console.ResetColor();
                    employee.GraphicEmployee();

                }


                Console.Write("\n\nNumber of employee (0 to exit): ");
                bool correctNumber = int.TryParse(Console.ReadLine(), out int number);

                if (number == 0 && correctNumber)
                    break;
                else if (number <= count && number > 0 && correctNumber)
                {
                    Employee employee = listEmployees[number - 1];
                    EmployeeModifyingData(ref employee, listEmployees);
                    listEmployees[number - 1] = employee;

                    string system = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    system = Path.Combine(system, "WareHouse", "Employee.json");
                    string json = JsonConvert.SerializeObject(listEmployees);
                    File.WriteAllText(system, json);
                }
            } while (true);
        }// Modifying employee
    }
}

