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

        List<Employee> employees = new List<Employee>();
        List<Product> listOfProducts = new List<Product>();
        Employee employee = new Employee();


        string systemOperation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WareHouse");
        Utils.FirstTimeUsing(ref listOfProducts, ref systemOperation, ref employees, ref firstTime);


        EmployeeMethods.ChoosingEmployee(ref employees,ref employee,firstTime);
        firstTime = false;

        systemOperation = Path.Combine(systemOperation, "Products.json");

        do
        {
            switch (employee.Position)
            {
                case PositionName.Admin:
                    do
                    {
                        correctNumber = true;
                        Console.Clear();
                        Console.WriteLine("1.\tAdd product\n2. \tRemoving product\n3. \tReports and sorting\n4. \tModifying Product\n5. \tHistory of Modifying\n6. \tStatistics\n7. \tAdding new employee\n8. \tModifying employees\n9. \tLoad JSON file\n10. \tLog out\n0. \tExit");
                        Console.Write($"\n\nPosition: {employee.Position}\nName: {employee.Name}\nLast Name: {employee.LastName}\n\nNumber: ");
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
                                ModificationsAndHistory.ModifyingReportHistory(ref listOfProducts, systemOperation,employee);
                                break;

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
                                Utils.JsonFileLoad(ref listOfProducts);
                                break;
                            case 10:
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
                        correctNumber = true;
                        Console.Clear();
                        Console.WriteLine("1. \tAdd product\n2. \tRemoving product\n3. \tModifying Product\n4. \tLoad JSON file\n5. \tLog out\n6. \tExit");
                        Console.Write($"\n\nPosition: {employee.Position}\nName: {employee.Name}\nLast Name: {employee.LastName}\n\nNumber: ");
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
                                Utils.JsonFileLoad(ref listOfProducts);
                                break;

                            case 5:
                                EmployeeMethods.ChoosingEmployee(ref employees, ref employee,firstTime);
                                break;

                            case 6:
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
                        correctNumber = true;
                        Console.Clear();
                        Console.WriteLine("1. \tAdd product\n2. \tRemoving product\n3. \tReports and sorting\n4. \tModifying Product\n5. \tHistory of Modifying\n6. \tLoad JSON file\n7. \tLog out\n8. \tExit");
                        Console.Write($"\n\nPosition: {employee.Position}\nName: {employee.Name}\nLast Name: {employee.LastName}\n\nNumber: ");
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
                                Utils.JsonFileLoad(ref listOfProducts);
                                break;

                            case 7:
                                EmployeeMethods.ChoosingEmployee(ref employees, ref employee,firstTime);
                                break;

                            case 8:
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
                        correctNumber = true;
                        Console.Clear();
                        Console.WriteLine("1. \tNew delivery\n2. \tLog out\n3. \tExit");
                        Console.Write($"\n\nPosition: {employee.Position}\nName: {employee.Name}\nLast Name: {employee.LastName}\n\nNumber: ");
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