using System;
using Newtonsoft.Json;

namespace Warehouse_Application;
internal class Program
{
    private static void Main(string[] args)
    {
        bool firstTime = true; 
        int number;
        bool closeProgram = false, correctNumber = false;
        string systemOperation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        systemOperation = Path.Combine(systemOperation, "WareHouse");

        List<Employee> employees = new List<Employee>();
        List<Product> listOfProducts = new List<Product>();
        Utils.FirstTimeUsing(ref listOfProducts, ref systemOperation, ref employees, ref firstTime);

        Employee employee = new Employee();

        EmployeeMethods.ChoosingEmployee(ref employees,ref employee,firstTime);
        firstTime = false;

        systemOperation = Path.Combine(systemOperation, "Products.json");

        do
        {
            switch(employee.Position)
            {
                case PositionName.Admin:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1. Add product\n2. Removing product\n3. Reports and sorting\n4. Modifying Product\n5. History of Modifying\n6. Statistics\n7. Adding new employee\n8. Edit employee\n9. Change position\n0. Exit");
                        Console.Write($"\n\nPosition: {employee.Position}\n\nNumber: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch (number)
                        {
                            case 1:
                                Utils.AddingProduct(ref listOfProducts, employee);
                                break;

                            case 2:
                                Utils.RemovingRecord(ref listOfProducts);
                                break;

                            case 3:
                                ReportMethods.ReportOfProducts(ref listOfProducts);
                                break;

                            case 4:
                                ModificationsAndHistory.ModifyingProduct(ref listOfProducts,employee);
                                break;

                            case 5:
                                ModificationsAndHistory.ModifyingReportHistory(ref listOfProducts, systemOperation,employee);                                break;

                            case 6:
                                Utils.Statistics(listOfProducts);
                                break;

                            case 7:
                                EmployeeMethods.AddingEmployee(ref employees,firstTime);
                                break;

                            case 8:
                                EmployeeMethods.EmployeeModifying(ref employees);
                                break;

                            case 9:
                                EmployeeMethods.ChoosingEmployee(ref employees,ref employee,firstTime);
                                break;

                            case 0:
                                closeProgram = true;
                                break;

                            default:
                                correctNumber = false;
                                break;
                        }
                    } while (!correctNumber);
                    break;

                case PositionName.Employee:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1. Add product\n2. Removing product\n3. Modifying Product\n4. Change position\n5. Exit");
                        Console.Write("Number: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch(number)
                        {
                            case 1:
                                Utils.AddingProduct(ref listOfProducts,employee);
                                break;

                            case 2:
                                Utils.RemovingRecord(ref listOfProducts);
                                break;

                            case 3:
                                ModificationsAndHistory.ModifyingProduct(ref listOfProducts,employee);
                                break;

                            case 4:
                                EmployeeMethods.ChoosingEmployee(ref employees, ref employee,firstTime);
                                break;

                            case 5:
                                closeProgram = true;
                                break;

                            default:
                                correctNumber = false;
                                break;

                        }
                    } while (!correctNumber);
                    break;

                case PositionName.Manager:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1. Add product\n2. Removing product\n3. Reports and sorting\n4. Modifying Product\n5. History of Modifying\n6. Change position\n7. Exit");
                        Console.Write("Number: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch(number)
                        {
                            case 1:
                                Utils.AddingProduct(ref listOfProducts, employee);
                                break;

                            case 2:
                                Utils.RemovingRecord(ref listOfProducts);
                                break;

                            case 3:
                                ReportMethods.ReportOfProducts(ref listOfProducts);
                                break;

                            case 4:
                                ModificationsAndHistory.ModifyingProduct(ref listOfProducts,employee);
                                break;

                            case 5:
                                ModificationsAndHistory.ModifyingReportHistory(ref listOfProducts, systemOperation,employee);
                                break;

                            case 6:
                                EmployeeMethods.ChoosingEmployee(ref employees, ref employee,firstTime);
                                break;

                            case 7:
                                closeProgram = true;
                                break;

                            default:
                                correctNumber = false;
                                break;


                        }
                    } while (!correctNumber);
                    break;
                case PositionName.Supplier:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1. New delivery\n2. Change position\n3. Exit");
                        Console.Write("Number: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch (number)
                        {
                            case 1:
                                ModificationsAndHistory.NewDelivery(ref listOfProducts, employee);
                                break;

                            case 2:
                                EmployeeMethods.ChoosingEmployee(ref employees, ref employee, firstTime);
                                break;

                            case 3:
                                closeProgram = true;
                                break;

                            default:
                                correctNumber = false;
                                break;
                        }
                    } while (!correctNumber);
                    break;
            }
            

        } while (!closeProgram);

    }
    public static void JsonFileRecord(ref List<Product> products) /// Method to overwriting list of Products
    {
        string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        systemOp = Path.Combine(systemOp, "WareHouse", "Products.json");
        string jsonWriter = JsonConvert.SerializeObject(products);
        File.WriteAllText(systemOp, jsonWriter);

        string jsonReader = File.ReadAllText(systemOp);
        products = JsonConvert.DeserializeObject<List<Product>>(jsonReader).ToList();
    }
}