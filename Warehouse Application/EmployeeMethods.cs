﻿using System;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Warehouse_Application
{
	public static class EmployeeMethods
	{
        private static void AcceptingModify(Employee e1, out bool accpet)
        {
            bool infinity = false;
            do
            {
                Console.WriteLine($"Name: {e1.Name}");
                Console.WriteLine($"Last name: {e1.LastName}");
                Console.WriteLine($"Id: {e1.Id}");
                Console.WriteLine($"Age: {e1.Age}");
                Console.WriteLine($"Position: {e1.Position}");
                Console.WriteLine($"Login: {e1.Login}");
                Console.WriteLine($"Password {e1.Password}");
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
        public static void ChoosingEmployee(ref List<Employee> employees, ref Employee employee, bool firsTime)
        {
            bool closeEmployee = false;
            do
            {
                try
                {
                    Console.Clear();
                    if (string.IsNullOrEmpty(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WareHouse", "Employee.json"))))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("Welcome to warehouse app. Please provide information to the main admin");
                        Console.ResetColor();

                        Console.WriteLine("\n\nAdmin Information");
                        employee.Position = PositionName.Admin;

                        NewEmployeeInformation(employee,ref employees, firsTime);
                    }
                    

                        Console.Clear();
                        int count = 0;
                        foreach (var worker in employees)
                        {
                            ++count;
                            Console.WriteLine($"{count}. {worker.Name} {worker.LastName} {worker.Position}");
                        }
                        Console.WriteLine("0. Exit");
                        Console.Write("\nNumber: ");
                        bool correctNumber = int.TryParse(Console.ReadLine(), out int number);

                    if (number == 0)
                        Environment.Exit(0);
                    else if (!correctNumber || count > number)
                        continue;

                        employee = employees[number - 1];

                        Console.Clear();

                        Console.WriteLine("LOGIN");
                        Console.Write("\n\nLogin: ");
                        string login = Console.ReadLine();
                        Console.Write("Password: ");
                        string password = Console.ReadLine();
                        if (employee.Login == login && employee.Password == password)
                        {
                            closeEmployee = true;
                        }
                        else
                            throw new FormatException("Wrong login or password");
                        employee.mainAccount = true;
                    
                }
                catch (FormatException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n" + e.Message);
                    Console.ResetColor();
                    Console.WriteLine("Click enter to continue");
                    Console.ReadKey();
                }
            } while (!closeEmployee);
        }
        public static void AddingEmployee(ref List<Employee> employees,bool firsTime)
        {
            Employee employee = new Employee();
            bool choosingPosition = false;
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

                    NewEmployeeInformation(employee,ref employees,firsTime);

                    choosingPosition = true;

                }
                catch (FormatException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(e.Message);
                    Console.ResetColor();
                    Console.WriteLine("Click enter to continue");
                    Console.ReadKey();
                }
            } while (!choosingPosition);


        }
        private static void NewEmployeeInformation(Employee employee,ref List<Employee> employees, bool firsTime)
        {
            Console.Write($"Name: ");
            string name = Console.ReadLine();
            employee.Name = name;

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();
            employee.LastName = lastName;

            Console.Write("Id (3 chars): ");
            string id = Console.ReadLine();
            employee.Id = id;

            Console.Write("Age: ");
            bool x = int.TryParse(Console.ReadLine(), out int age);
            if (!x)
                throw new FormatException("Wrong age");
            employee.Age = age;

            bool correctLogin = false;
            string login = name.Substring(0, 3);
            do
            {
                for (int i = 0; i < 3; i++)
                {
                    Random random = new Random();
                    int p = random.Next(0, 10);
                    login += p.ToString();
                }
                if (firsTime || string.IsNullOrEmpty(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"WareHouse","Employee.json"))))
                {
                    break;
                }
                else if (!employees.Any(x => x.Login == login))
                    correctLogin = true; /// dodac throw bad login

            } while (!correctLogin);

            Console.WriteLine($"Login: {login}");
            employee.Login = login;

            Console.Write("Password: ");
            string password = Console.ReadLine();
            employee.Password = password;
            employee.mainAccount = false;

            string json;
            List<Employee> firsTimeList = new List<Employee>();

            Console.Clear();
            Console.WriteLine("WRITE DOWN THIS INFORMATION:");
            Console.WriteLine($"Login: {employee.Login}\nPassword: {employee.Password}\n\nClick enter to continue");
            Console.ReadKey();
            if(firsTime)
            {
                firsTimeList.Add(employee);
                json = JsonConvert.SerializeObject(firsTimeList);
            }
            else
            {
                employees.Add(employee);
                json = JsonConvert.SerializeObject(employees);
            }
            ///
            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.WriteAllText(Path.Combine(systemOp,"WareHouse" ,"Employee.json"), json);

            string jsonReader = File.ReadAllText(Path.Combine(systemOp, "WareHouse", "Employee.json"));
            employees = JsonConvert.DeserializeObject<List<Employee>>(jsonReader);

        }
        public static void EmployeeModifying(ref List<Employee> listEmployees)
        {
            bool infinti = false;
            bool correctNumber = false;
            bool correctChoosing = false;
            do
            {
                Console.Clear();
                bool remove;
                Console.Write("1.Remove\n2.Modifying\n3.Exit\nNumber: ");
                bool correctNumberChoose = int.TryParse(Console.ReadLine(), out int choose);
                Console.Clear();

                if (correctNumberChoose && choose == 1)
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
                            Console.WriteLine($"Name: {employee.Name}\nLast Name: {employee.LastName}\nId: {employee.Id}\nAge: {employee.Age}\nPosition: {employee.Position}\nLogin: {employee.Login}\nPassword: {employee.Password}");
                        }
                        Console.WriteLine("Number of employee (0 to exit): ");
                        correctNumber = int.TryParse(Console.ReadLine(), out int number);

                        if (correctNumber && number == 0)
                        {
                            correctChoosing = true;
                            break;
                        }
                        else if (correctNumber && number > 0 && number <= count)
                        {
                            if (listEmployees[number-1].mainAccount != true)
                            {
                                listEmployees.RemoveAt(number - 1);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Its main Admin. You can't remove this position\nClick enter to continue!");
                                Console.ReadKey();
                            }
                        }

                    } while (!infinti);

                }
                else if (correctNumberChoose && choose == 2)
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
                            Console.WriteLine($"Name: {employee.Name}\nLast Name: {employee.LastName}\nId: {employee.Id}\nAge: {employee.Age}\nPosition: {employee.Position}\nLogin: {employee.Login}\nPassword: {employee.Password}");
                        }


                        Console.WriteLine("Number of employee (0 to exit): ");
                        correctNumber = int.TryParse(Console.ReadLine(), out int number);

                        if (number == 0)
                        {
                            correctChoosing = true;
                            break;
                        }
                        else if (number <= count && number > 0)
                        {
                            Employee employee = listEmployees[number - 1];
                            EmployeeModifying(ref employee, listEmployees);
                            listEmployees[number - 1] = employee;

                            string system = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            system = Path.Combine(system, "WareHouse", "Employee.json");
                            string json = JsonConvert.SerializeObject(listEmployees);
                            File.WriteAllText(system, json);
                        }
                    } while (!infinti);
                }
                else if (correctNumberChoose && choose == 3)
                {
                    return;
                }

            } while (!correctChoosing);
        }
        private static void EmployeeModifying(ref Employee employee, List<Employee> employees)
        {
            Employee copy = employee;
            string property = "", value = "";
            bool correctModify = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Employee Modifications\n1.Name\n2.Last name\n3.Id\n4.Age\n5.Position\n6.Password\n7.Login\n8.Remove Employee\n9.Main account\n10.Exit");
                Console.SetCursorPosition(25, 1);
                Console.WriteLine($"Name: {employee.Name}");
                Console.SetCursorPosition(25, 2);
                Console.WriteLine($"Last name: {employee.LastName}");
                Console.SetCursorPosition(25, 3);
                Console.WriteLine($"Id: {employee.Id}");
                Console.SetCursorPosition(25, 4);
                Console.WriteLine($"Age: {employee.Age}");
                Console.SetCursorPosition(25, 5);
                Console.WriteLine($"Position: {employee.Position}");
                Console.SetCursorPosition(25, 6);
                Console.WriteLine($"Password: {employee.Password}");
                Console.SetCursorPosition(25, 7);
                Console.WriteLine($"Login: {employee.Login}");
                Console.SetCursorPosition(25, 8);
                Console.WriteLine($"Main account {employee.mainAccount}");
                Console.SetCursorPosition(0, 11);
                Console.Write("Number: ");

                string answer = Console.ReadLine();
                switch(answer)
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
                        break;
                }
            } while (!correctModify);


            if(property == "Position")
            {
                bool x = true;
                do
                {
                    x = true;
                    Console.Clear();
                    Console.Write("1.Admin\n2.Supplier\n3.Employee\n4.Manager\n5.Exit\nNumber: ");
                    string answer = Console.ReadLine();
                    switch(answer)
                    {
                        case "1":
                            copy.Position = PositionName.Admin;
                            break;
                        case "2":
                            copy.Position = PositionName.Supplier;
                            break;
                        case "3":
                            copy.Position = PositionName.Employee;
                            break;
                        case "4":
                            copy.Position = PositionName.Manager;
                            break;
                        case "5":
                            return;
                        default:
                            x = false;
                            break;
                            
                    }
                } while (!x);
            }
            else if(property == "mainAccount")
            {
                bool x = true;
                do
                {
                    x = true;
                    Console.Clear();
                    Console.WriteLine("1.Main account (true)\n2.Main account (false)\n3.Exit");
                    string answer = Console.ReadLine();
                    switch(answer)
                    {
                        case "1":
                            copy.mainAccount = true;
                            break;
                        case "2":
                            copy.mainAccount = false;
                            break;
                        case "3":
                            return;
                        default:
                            break;

                    }
                } while (!x);
            }
            else
            {
                Console.Write($"Changing {property}: ");
                value = Console.ReadLine();
            }

            bool accept;

            PropertyInfo propertyInfo = copy.GetType().GetProperty(property);
            object valueParsed = ModificationsAndHistory.ParseValue(value, propertyInfo.PropertyType);
            copy.GetType().GetProperty(property).SetValue(copy, valueParsed);

            if(employees.Any(x => x.Id == valueParsed)&& property == "Id")
            {
                Console.WriteLine("This id is already exist\nClick enter to continue!");
                Console.ReadKey();
                return;
            }

            AcceptingModify(copy, out accept);
            if(accept)
            {
                employee = copy;
            }

        }
    }
}

