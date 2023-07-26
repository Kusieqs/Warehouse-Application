using Warehouse_Application;
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
                Console.WriteLine("1. Add product\n2. Products report");
                Console.Write("Number: ");
                correctNumber = int.TryParse(Console.ReadLine(), out number);
            } while (!correctNumber);

            switch (number)
            {
                case 1:
                    Utils.AddingProduct(ref listOfProducts);
                    Console.Clear();
                    break;

                case 2:
                    Utils.RaportOfProducts(listOfProducts);
                    break;

                default:
                    break;
            }


        } while (!closeProgram);
    }
}