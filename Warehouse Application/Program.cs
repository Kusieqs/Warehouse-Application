using System;
using Newtonsoft.Json;

namespace Warehouse_Application;
internal class Program
{
    private static void Main(string[] args)
    {
        bool firstTime = true; 
        int number;
        bool closeProgram = false;

        List<Employee> employees = new List<Employee>();
        List<Product> listOfProducts = new List<Product>();
        Employee employee = new Employee();


        string systemOperation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WareHouse");
        Utils.FirstTimeUsing(listOfProducts, ref systemOperation,employees, ref firstTime);

        EmployeeMethods.ChoosingEmployee(ref employees,ref employee,firstTime);
        firstTime = false;
        systemOperation = Path.Combine(systemOperation, "Products.json");

        if(!string.IsNullOrEmpty(File.ReadAllText(systemOperation)))
        {
            string jsonWriter = File.ReadAllText(systemOperation);
            listOfProducts = JsonConvert.DeserializeObject<List<Product>>(jsonWriter).ToList();
        }
        else
        {
            listOfProducts = JsonConvert.DeserializeObject<List<Product>>("[]").ToList();
        }
        do
        {
            bool correctNumber = false;
            switch (employee.Position)
            {
                case PositionName.Admin:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1.\tAdd product\n2. \tRemoving product\n3. \tReports and sorting\n4. \tModifying Product\n5. \tHistory of Modifying\n6. \tStatistics\n7. \tAdding new employee\n8. \tModifying employees\n9. \tLoad JSON file\n10. \tLog out\n0. \tExit");
                        Console.Write($"\n\nPosition: {employee.Position}\nName: {employee.Name}\nLast Name: {employee.LastName}\n\nNumber: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch (number)
                        {
                            case 1:
                                Utils.AddingProduct(listOfProducts, employee);
                                break;

                            case 2:
                                Utils.RemovingRecord(listOfProducts);
                                break;

                            case 3:
                                ReportMethods.ReportOfProducts(listOfProducts);
                                break;

                            case 4:
                                ModificationsAndHistory.ModifyingProduct(listOfProducts,employee);
                                break;

                            case 5:
                                ModificationsAndHistory.ModifyingReportHistory(listOfProducts, employee);
                                JsonFileRecord(listOfProducts);
                                break;

                            case 6:
                                Utils.Statistics(listOfProducts);
                                break;

                            case 7:
                                EmployeeMethods.AddingEmployee(employees,firstTime);
                                break;

                            case 8:
                                EmployeeMethods.MenuOfEmployee(employees,employee.Id);
                                break;

                            case 9:
                                Utils.JsonFileLoad(listOfProducts);
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
                        Console.Clear();
                        Console.WriteLine("1. \tAdd product\n2. \tRemoving product\n3. \tModifying Product\n4. \tLoad JSON file\n5. \tLog out\n6. \tExit");
                        Console.Write($"\n\nPosition: {employee.Position}\nName: {employee.Name}\nLast Name: {employee.LastName}\n\nNumber: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch(number)
                        {
                            case 1:
                                Utils.AddingProduct(listOfProducts,employee);
                                break;

                            case 2:
                                Utils.RemovingRecord(listOfProducts);
                                break;

                            case 3:
                                ModificationsAndHistory.ModifyingProduct(listOfProducts,employee);
                                break;

                            case 4:
                                Utils.JsonFileLoad(listOfProducts);
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
                        Console.Clear();
                        Console.WriteLine("1. \tAdd product\n2. \tRemoving product\n3. \tReports and sorting\n4. \tModifying Product\n5. \tHistory of Modifying\n6. \tLoad JSON file\n7. \tLog out\n8. \tExit");
                        Console.Write($"\n\nPosition: {employee.Position}\nName: {employee.Name}\nLast Name: {employee.LastName}\n\nNumber: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch(number)
                        {
                            case 1:
                                Utils.AddingProduct(listOfProducts, employee);
                                break;

                            case 2:
                                Utils.RemovingRecord(listOfProducts);
                                break;

                            case 3:
                                ReportMethods.ReportOfProducts(listOfProducts);
                                break;

                            case 4:
                                ModificationsAndHistory.ModifyingProduct(listOfProducts,employee);
                                break;

                            case 5:
                                ModificationsAndHistory.ModifyingReportHistory(listOfProducts, employee);
                                JsonFileRecord(listOfProducts);
                                break;

                            case 6:
                                Utils.JsonFileLoad(listOfProducts);
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
                        Console.Clear();
                        Console.WriteLine("1. \tNew delivery\n2. \tLog out\n3. \tExit");
                        Console.Write($"\n\nPosition: {employee.Position}\nName: {employee.Name}\nLast Name: {employee.LastName}\n\nNumber: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch (number)
                        {
                            case 1:
                                ModificationsAndHistory.NewDelivery(listOfProducts, employee);
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
    public static void JsonFileRecord(List<Product> products) /// Method to overwriting list of Products
    {
        string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        systemOp = Path.Combine(systemOp, "WareHouse", "Products.json");
        string jsonWriter = JsonConvert.SerializeObject(products);
        File.WriteAllText(systemOp, jsonWriter);

        string jsonReader = File.ReadAllText(systemOp);
        products = JsonConvert.DeserializeObject<List<Product>>(jsonReader).ToList();
    }
}