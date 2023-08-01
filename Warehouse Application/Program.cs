using Warehouse_Application;
using System.Text.RegularExpressions;
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
                Console.WriteLine("1. Add product\n2. Removing product\n3. Reports\n4.Modifying the product\n5. Exit");
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
                    ReportMethods.ReportOfProducts(listOfProducts,systemOperation);
                    break;

                case 4:
                    break;

                case 5:
                    closeProgram = true;
                    break;

                default:
                    break;
            }

        } while (!closeProgram);
    }
}