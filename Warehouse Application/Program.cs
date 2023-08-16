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

        Employee employee = null;

        Utils.AddingEmployee(ref employees, out employee);

        systemOperation = Path.Combine(systemOperation, "Products.json");
        do
        {
            do
            {
                Console.Clear();
                Console.WriteLine("1. Add product\n2. Removing product\n3. Reports\n4. Modifying Product\n5. History of Modifying\n6. Statistics\n7. Exit");
                Console.Write("Number: ");
                correctNumber = int.TryParse(Console.ReadLine(), out number);
            } while (!correctNumber);

            switch (number)
            {
                case 1:
                    Utils.AddingProduct(ref listOfProducts, systemOperation);
                    Console.Clear();
                    break;

                case 2:
                    Utils.RemovingRecord(ref listOfProducts, systemOperation);
                    break;

                case 3:
                    ReportMethods.ReportOfProducts(ref listOfProducts,systemOperation);
                    break;

                case 4:
                    ModificationsAndHistory.ModifyingProduct(ref listOfProducts,systemOperation);   
                    break;

                case 5:
                    ModificationsAndHistory.ModifyingReportHistory(ref listOfProducts,systemOperation);
                    JsonFileRecord(ref listOfProducts, systemOperation);
                    break;

                case 6:
                    Utils.Statistics(listOfProducts);
                    break;
                case 7:
                    closeProgram = true;
                    break;

                default:
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