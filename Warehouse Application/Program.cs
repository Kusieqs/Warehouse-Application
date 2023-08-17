﻿using System;
using Newtonsoft.Json;

namespace Warehouse_Application;
internal class Program
{
    private static void Main(string[] args)
    {
        int number;
        bool closeProgram = false, correctNumber, closeEmployee = false;
        string systemOperation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        systemOperation = Path.Combine(systemOperation, "WareHouse");

        List<Employee> employees = new List<Employee>();
        List<Product> listOfProducts = new List<Product>();
        Utils.FirstTimeUsing(ref listOfProducts, ref systemOperation, ref employees);

        Employee employee;

        Utils.ChoosingEmployee(ref employees, out employee);

        systemOperation = Path.Combine(systemOperation, "Products.json");

        do
        {
            switch(employee.Position)
            {
                case PositionName.Admin:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1. Add product\n2. Removing product\n3. Reports\n4. Modifying Product\n5. History of Modifying\n6. Statistics\n7.Adding new employee\n8.Edit employee\n8. Exit");
                        Console.Write("Number: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch (number)
                        {
                            case 1:
                                Utils.AddingProduct(ref listOfProducts, systemOperation);
                                break;

                            case 2:
                                Utils.RemovingRecord(ref listOfProducts, systemOperation);
                                break;

                            case 3:
                                ReportMethods.ReportOfProducts(ref listOfProducts, systemOperation);
                                break;

                            case 4:
                                ModificationsAndHistory.ModifyingProduct(ref listOfProducts, systemOperation);
                                break;

                            case 5:
                                ModificationsAndHistory.ModifyingReportHistory(ref listOfProducts, systemOperation);
                                JsonFileRecord(ref listOfProducts, systemOperation);
                                break;

                            case 6:
                                Utils.Statistics(listOfProducts);
                                break;

                            case 7:
                                Utils.AddingEmployee(ref employees);
                                break;

                            case 8:
                                

                            case 9:
                                closeProgram = true;
                                break;

                            default:
                                break;
                        }
                    } while (!correctNumber);
                    break;

                case PositionName.Employee:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1. Add product\n2. Removing product\n3. Modifying Product\n4. Exit");
                        Console.Write("Number: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch(number)
                        {
                            case 1:
                                Utils.AddingProduct(ref listOfProducts, systemOperation);
                                break;

                            case 2:
                                Utils.RemovingRecord(ref listOfProducts, systemOperation);
                                break;

                            case 3:
                                ModificationsAndHistory.ModifyingProduct(ref listOfProducts, systemOperation);
                                break;

                            case 4:
                                closeProgram = true;
                                break;

                            default:
                                break;

                        }
                    } while (!correctNumber);
                    break;

                case PositionName.Manager:
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1. Add product\n2. Removing product\n3. Reports\n4. Modifying Product\n5. History of Modifying\n6. Exit");
                        Console.Write("Number: ");
                        correctNumber = int.TryParse(Console.ReadLine(), out number);
                        switch(number)
                        {
                            case 1:
                                Utils.AddingProduct(ref listOfProducts, systemOperation);
                                break;

                            case 2:
                                Utils.RemovingRecord(ref listOfProducts, systemOperation);
                                break;

                            case 3:
                                ReportMethods.ReportOfProducts(ref listOfProducts, systemOperation);
                                break;

                            case 4:
                                ModificationsAndHistory.ModifyingProduct(ref listOfProducts, systemOperation);
                                break;

                            case 5:
                                ModificationsAndHistory.ModifyingReportHistory(ref listOfProducts, systemOperation);
                                JsonFileRecord(ref listOfProducts, systemOperation);
                                break;

                            case 6:
                                closeProgram = true;
                                break;

                            default:
                                break;


                        }
                    } while (!correctNumber);
                    break;

                case PositionName.Supplier:
                    break;
            }
            

        } while (!closeProgram);
        JsonFileRecord(ref listOfProducts, systemOperation);

    }
    public static void JsonFileRecord(ref List<Product> products, string systemOp)
    {
        string jsonWriter = JsonConvert.SerializeObject(products);
        File.WriteAllText(systemOp, jsonWriter);

        string jsonReader = File.ReadAllText(systemOp);
        products = JsonConvert.DeserializeObject<List<Product>>(jsonReader).ToList();
    }
}