using System;
using Newtonsoft.Json;
namespace Warehouse_Application
{
	public static class Utils
	{
		public static void FirstTimeUsing(ref List<Product> products,ref string systemOperation)
		{
			systemOperation = Path.Combine(systemOperation, "Products.json");

			if (!File.Exists(systemOperation))
			{
				string jsonWriter = JsonConvert.SerializeObject("");
				File.WriteAllText(systemOperation, jsonWriter);
			}
			else
			{
				string jsonReader = File.ReadAllText(systemOperation);
				products = JsonConvert.DeserializeObject<List<Product>>(systemOperation);
			}
		}
		public static void AddingProduct(ref List<Product> products)
		{

			bool correctPrice, correctQuantity, correctData = false;
			string name, id;
			int quantity;
			double price;
			DateTime date = DateTime.Now;

			do
			{
				Console.Clear();
				Console.Write("Name of product: ");
				name = Console.ReadLine().Trim();

				Console.Write("\nPrice of product: ");
				correctPrice = double.TryParse(Console.ReadLine(), out price);

				Console.Write("\nQuantity of product: ");
				correctQuantity = int.TryParse(Console.ReadLine(), out quantity);

				Console.Write("\nId of product (First 4 letters and 5 numbers, example - AbcD12345 )");
				id = Console.ReadLine().Trim();

				if(correctData && correctQuantity)
				{
					try
					{
						Product p1 = new Product(name, id, price, quantity, date);
						correctData = true;
						products.Add(p1);

					}
					catch(FormatException e)
					{
						Console.WriteLine(e.Message);
					}
					finally
					{
						Console.WriteLine("Click enter to continue");
						Console.ReadKey();
                    }
				}
				else
				{
					Console.WriteLine("Information about product are not correct\nClick enter to continue");
					Console.ReadKey();
				}
			} while (!correctData);

		}
		public static void RaportOfProducts(List<Product> products)
		{
			bool endOfRaport = false;
			do
			{
				Console.Clear();
				Console.WriteLine("RAPORTS:");
				Console.WriteLine("1.All Products\n2.Searching by infomration\n3.Sort by values\n4.Exit");
				string answer = Console.ReadLine();
				switch (answer)
				{
					case "1":
						break;
					case "2":
						break;
					case "3":
						break;
					case "4":
						endOfRaport = true;
						break;
					default:
						break;

				}
			} while (!endOfRaport);
		}
	}
}

