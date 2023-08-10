using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Warehouse_Application
{
	public static class Utils
	{
		public static void FirstTimeUsing(ref List<Product> products,ref string systemOperation)
		{
            if (File.Exists(systemOperation) && string.IsNullOrEmpty(File.ReadAllText(Path.Combine(systemOperation, "Products.json"))))
            {
                try
                {
                    File.Delete(systemOperation);
                }
                catch (IOException e)
                {
                    Console.WriteLine("Wystapil blad podczas usówania pliku: " + e);
                }
            }

            systemOperation = Path.Combine(systemOperation, "Products.json");

			if (!File.Exists(systemOperation))
			{
				string jsonWriter = string.Empty;
				File.WriteAllText(systemOperation, jsonWriter);
			}
			else
			{
				string jsonReader = File.ReadAllText(systemOperation);
				products = JsonConvert.DeserializeObject<List<Product>>(jsonReader);
			}
		}
		public static void AddingProduct(ref List<Product> products,string systemOp)
		{

			bool correctPrice, correctQuantity, correctData = false;
			string name, id;
			int quantity;
			double price;
            DateTime copyDate = DateTime.Now;
            DateTime date = copyDate.Date;


			do
			{
				Console.Clear();
				Console.Write("Name of product: ");
				name = Console.ReadLine().Trim();

				Console.Write("\nPrice of product: ");
				correctPrice = double.TryParse(Console.ReadLine(), out price);

				Console.Write("\nQuantity of product: ");
				correctQuantity = int.TryParse(Console.ReadLine(), out quantity);

				Console.Write("\nId of product (First 4 letters and 5 numbers, example - AbcD12345): ");
				id = Console.ReadLine().Trim();


                try
                {
                    Product p1 = new Product(name, id, price, quantity, date);

                    string jsonCreator;
                    correctData = true;
                    if (string.IsNullOrEmpty(File.ReadAllText(systemOp)))
                    {
                        List<Product> products1Copy = new List<Product>();
                        products1Copy.Add(p1);
                        jsonCreator = JsonConvert.SerializeObject(products1Copy);
                        File.WriteAllText(systemOp, jsonCreator);
                    }
                    else
                    {
                        products.Add(p1);
                        jsonCreator = JsonConvert.SerializeObject(products);
                        File.WriteAllText(systemOp, jsonCreator);

                    }
                    correctData = true;
                }
                catch (FormatException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n{e.Message}");
                    Console.ResetColor();
                    Console.WriteLine("Click enter to continue");
                    Console.ReadKey();
                }

            } while (!correctData);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nProduct added to list");
            Console.ResetColor();
            Console.WriteLine("Click enter to continue");
            Console.ReadKey();

			string jsonWriter = File.ReadAllText(systemOp);
			products = JsonConvert.DeserializeObject<List<Product>>(jsonWriter);

		}
		public static void RecordingTxtFile(string systemOp, string report)
		{
            if (!string.IsNullOrEmpty(systemOp))
            {
                Console.Clear();
                Console.Write("File Name: ");
                string fileName = Console.ReadLine() + ".txt";
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                path = Path.Combine(path, "Desktop", fileName);
                File.WriteAllText(path, report);
                Console.WriteLine("File is complete!");
            }
            else
            {
                Console.WriteLine("File is empty!\nClick enter to continue");
                Console.ReadKey();
            }
        }
        public static void GraphicRemovingAndModifying(List<Product> products, out string answer, out bool correctNumber, out int number, ref bool graphic)
        {
            graphic = true;
            Console.Clear();
            int count = 0;
            foreach (var product in products)
            {
                count++;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Number: " + count);
                Console.ResetColor();
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: {product.Price}");
                Console.WriteLine($"Quantity: {product.Quantity}");
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Date: {product.date}");
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("\n                    \n");
                Console.ResetColor();
            }
            Console.Write("\nWrite number of product or Id (4 Letters and 5 numbers or 0 to exit)\nNumber or id: ");
            answer = Console.ReadLine();
            correctNumber = int.TryParse(answer, out number);
            Console.Clear();

        }
        public static void RemovingRecord(ref List<Product> products, string systemOp)
        {
            Product p1 = new Product();
            string removingRecord;
            int number;
            bool itIsNumber, correctNumber, graphic = false;
            bool endRemovingRecord = false;
            if (products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("List is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            else
            {
                do
                {
                    GraphicRemovingAndModifying(products, out removingRecord, out correctNumber, out number, ref graphic);

                    if (correctNumber && number <= products.Count && number > 0)
                    {
                        p1 = products[number - 1];
                        itIsNumber = true;
                    }
                    else if (Regex.IsMatch(removingRecord, @"^[A-Za-z]{4}\d{5}$") && products.Any(x => x.Id == removingRecord))
                    {
                        p1 = products.Find(x => x.Id == removingRecord);
                        itIsNumber = false;
                    }
                    else if (correctNumber && number == 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong number or id\nClick enter to continue");
                        Console.ReadKey();
                        continue;
                    }
                    Console.Clear();
                    bool choosingCorrect = false;
                    do
                    {
                        Console.WriteLine($"Name: {p1.Name}");
                        Console.WriteLine($"Price: {p1.Price}");
                        Console.WriteLine($"Quantity: {p1.Quantity}");
                        Console.WriteLine($"Id: {p1.Id}");
                        Console.WriteLine($"Date: {p1.date}");
                        Console.WriteLine("\nDo you want to remove?\n1.Yes\n2.No");
                        Console.Write("Number: ");
                        string choosingYesNo = Console.ReadLine();

                        if (choosingYesNo == "1")
                        {
                            choosingCorrect = true;
                            if (itIsNumber)
                                products.RemoveAt(number - 1);
                            else
                                products.Remove(p1);
                            Program.JsonFileRecord(ref products, systemOp);

                        }
                        else if (choosingYesNo == "2")
                            choosingCorrect = true;


                    } while (!choosingCorrect);

                } while (!endRemovingRecord);

            }
       
        }
        public static string NameFile()
        {
            string x = "";
            bool attempt = false;
            do
            {
                Console.Clear();
                Console.Write("File Name: ");
                x = Console.ReadLine();
                if (x.Length > 0)
                    attempt = true;

            } while (!attempt);
            return x;
        }
    }
}

