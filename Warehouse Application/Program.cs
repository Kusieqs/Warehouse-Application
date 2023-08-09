using System;
using Newtonsoft.Json;

namespace Warehouse_Application;
internal class Program
{
    private static void Main(string[] args)
    {
        int number;
        bool closeProgram = false, correctNumber;
        string systemOperation = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        systemOperation = Path.Combine(systemOperation, "Desktop");
        List<Product> listOfProducts = new List<Product>();
        Utils.FirstTimeUsing(ref listOfProducts, ref systemOperation);

        do
        {
            do
            {
                Console.Clear();
                Console.WriteLine("1. Add product\n2. Removing product\n3. Reports\n4. Modifying Product\n5. History of Modifying\n6. Exit");
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
                    closeProgram = true;
                    break;

                default:
                    break;
            }

        } while (!closeProgram);

    }
    public static void JsonFileRecord(ref List<Product> products, string systemOp)
    {
        string jsonWriter = JsonConvert.SerializeObject(products);
        File.WriteAllText(systemOp, jsonWriter);

        string jsonReader = File.ReadAllText(systemOp);
        products = JsonConvert.DeserializeObject<List<Product>>(jsonReader).ToList();
    }
}