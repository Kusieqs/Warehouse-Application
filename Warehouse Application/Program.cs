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
                    ModificationsAndHistory.ModifyingReportHistory(ref listOfProducts);
                    break;

                case 6:
                    closeProgram = true;
                    break;

                default:
                    break;
            }

        } while (!closeProgram);
        JsonFile(ref listOfProducts, systemOperation);
    }
    public static void JsonFile(ref List<Product> p1, string systemOp)
    {
        string jsonCreator = JsonConvert.SerializeObject(p1);
        File.WriteAllText(systemOp, jsonCreator);

        string jsonWriter = File.ReadAllText(systemOp);
        p1 = JsonConvert.DeserializeObject<List<Product>>(jsonWriter);
    }
}