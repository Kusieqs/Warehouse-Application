﻿using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
namespace Warehouse_Application
{
	public static class Utils
	{
		public static void FirstTimeUsing(ref List<Product> products,ref string systemOperation)
		{
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

				if(correctPrice && correctQuantity)
				{
					try
					{
						string jsonCreator;
						Product p1 = new Product(name, id, price, quantity, date);
						correctData = true;
						if(string.IsNullOrEmpty(File.ReadAllText(systemOp)))
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
					Console.WriteLine("Test");
			} while (!correctData);

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
		public static void RemovingRecord(ref List<Product> products, string systemOp)
		{
			bool endRemovingRecord = false;
			do
			{
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
					Console.WriteLine("- - - - - - - - - - - -");
				}

				Product p1 = new Product();
				Console.Write("Write number of product or Id (4 Letters and 5 numbers) to remove product or 0 to exit\nNumber or id: ");
				string removingRecord = Console.ReadLine();
				bool correctNumber = int.TryParse(removingRecord, out int number);
				Console.Clear();
				bool itIsNumber;

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
				else if(correctNumber && number == 0)
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

                    string choosingYesNo = Console.ReadLine();

					if (choosingYesNo == "1")
					{
						choosingCorrect = true;
						if (itIsNumber)
							products.RemoveAt(number - 1);
						else
							products.Remove(p1);

						string jsonCreator = JsonConvert.SerializeObject(products);
						File.WriteAllText(systemOp, jsonCreator);

						string jsonWriter = File.ReadAllText(systemOp);
						products = JsonConvert.DeserializeObject<List<Product>>(jsonWriter);

					}
					else if (choosingYesNo == "2")
						choosingCorrect = true;


                } while (!choosingCorrect);

            } while (!endRemovingRecord);
		}
	}
}

