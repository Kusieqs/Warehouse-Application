﻿using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using OfficeOpenXml;
using System.IO;


namespace Warehouse_Application
{
    public static class ReportMethods
    {
        public static void ReportOfProducts(ref List<Product> products, string systemOp)
        {
            bool endOfRaport = false;
            if(products.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("List is empty\nClick enter to continue");
                Console.ReadKey();
                return;
            }
            do
            {
                Console.Clear();
                Console.WriteLine("REPORTS:");
                Console.Write("1.All Products\n2.Search by id\n3.Sort by values\n4.Exit\n\nNumber: ");
                string answer = Console.ReadLine();
                Console.Clear();
                switch (answer)
                {
                    case "1":
                        AllProductReport(products, systemOp);
                        break;
                    case "2":
                        SearchingById(products, systemOp);
                        break;
                    case "3":
                        SortingByValue(products, systemOp);
                        break;
                    case "4":
                        endOfRaport = true;
                        break;
                    default:
                        break;

                }
            } while (!endOfRaport);
        }
        private static void AllProductReport(List<Product> products, string systemOp)
        {
            bool endOfReport = false;
            do
            {
                string report = "";
                Console.WriteLine("REPORTS\n\n");
                Console.WriteLine("- - - - - - - - - - - -");
                foreach (var product in products)
                {
                    report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.date}\n - - - - - - - - \n";
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Price: {product.Price}");
                    Console.WriteLine($"Quantity: {product.Quantity}");
                    Console.WriteLine($"Id: {product.Id}");
                    Console.WriteLine($"Date: {product.date}");
                    Console.WriteLine("- - - - - - - - - - - -");
                }
                Console.WriteLine("\n\n");
                ReportMenu(systemOp, report, ref endOfReport,products);
            } while (!endOfReport);

        }
        private static void SearchingById(List<Product> products, string systemOp)
        {

            bool endSearching = false;
            do
            {
                Console.Clear();
                List<Product> copyList = products.ToList();
                Console.Write("Id: ");
                string idSearching = Console.ReadLine();
                if (Regex.IsMatch((idSearching), @"^[A-Za-z]{4}\d{5}$"))
                {
                    copyList = copyList.Where(x => x.Id == idSearching).ToList();
                    if (copyList.Count > 0)
                    {
                        Console.Clear();
                        string report = "";
                        foreach (var product in copyList)
                        {
                            report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.date}\n - - - - - - - - \n";
                            Console.WriteLine("Name: " + product.Name);
                            Console.WriteLine("Price: " + product.Price);
                            Console.WriteLine("Quantity: " + product.Quantity);
                            Console.WriteLine("Id: " + product.Id);
                            Console.WriteLine("Date: " + product.date);
                            endSearching = true;
                        }
                        Console.WriteLine("\n\n");
                        ReportMenu(systemOp, report, ref endSearching, copyList);

                    }
                    else
                    {
                        Console.WriteLine("\nId is not in the database\nClick enter to continue or 0 to exit");
                        string exitOrNot = Console.ReadLine();
                        if (exitOrNot == "0")
                            endSearching = true;
                    }
                }
                else
                {
                    Console.WriteLine("\nWrong id (4 Letters and 5 numbers, example: Abcd12345)\nClick enter to continue or 0 to exit");
                    string exitOrNot = Console.ReadLine();
                    if (exitOrNot == "0")
                        endSearching = true;
                }

            } while (!endSearching);
        }
        private static void SortingByValue(List<Product> products, string systemOp)
        {
            List<Product> copyList = new List<Product>();
            List<Product> sortList = new List<Product>();
            bool endOfSort = false, attempt = false;
            string sortingBy, operatorSort;
            DateTime dateSorting;
            string value = "";
            int year, month, day;
            do
            {
                do
                {
                    Console.Clear();
                    Console.Write("1.Sort by value price\n2.Sort by value quantity\n3.Sort by date\n4.Exit\n\nNumber: ");
                    sortingBy = Console.ReadLine();
                    switch (sortingBy)
                    {
                        case "1": sortingBy = "Price";
                            attempt = true;
                            break;
                        case "2": sortingBy = "Quantity";
                            attempt = true;
                            break;
                        case "3": sortingBy = "date"; ///.sprobowac sortowania po przez nie date tylko dni albo miesiace albo rok date.Day
                            attempt = true;
                            break;
                        case "4":
                            return;
                        default:
                            break;
                    }

                } while (!attempt);
                attempt = false;
                do
                {
                    Console.Clear();
                    if (sortingBy == "date")
                    {
                        do
                        {
                            bool yearBool, monthBool, dayBool;

                            Console.Clear();
                            Console.Write("Year: ");
                            yearBool = int.TryParse(Console.ReadLine(), out year);
                            Console.Write("Month: ");
                            monthBool = int.TryParse(Console.ReadLine(), out month);
                            Console.Write("Day: ");
                            dayBool = int.TryParse(Console.ReadLine(), out day);
                            if (yearBool && monthBool && dayBool)
                            {
                                if ((year < 1 || month < 1 || month > 12 || day < 1))
                                {
                                    int daysInMonth = DateTime.DaysInMonth(year, month);
                                    Console.WriteLine("Wrong Date\nClick enter to continue");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    int daysInMonth = DateTime.DaysInMonth(year, month);
                                    if (daysInMonth >= day)
                                    {
                                        dateSorting = new DateTime(year,month,day);
                                        value = dateSorting.ToString();
                                        attempt = true;
                                    }
                                }
                            }
                        } while (!attempt);
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write("Value: ");
                        attempt = double.TryParse(Console.ReadLine(), out double x);
                        value = x.ToString();
                        
                    }
                } while (!attempt);
                attempt = false;
                do
                {
                    Console.Clear();
                    Console.Write($"Choose one of this operators ( = , != , > , < , >= , <= )\n\n{sortingBy} [operator] {value}: ");
                    operatorSort = Console.ReadLine();
                    string[] operators = new string[] { "=", "!=", ">", "<", ">=", "<=" };
                    for (int i = 0; i < operators.Length; i++)
                    {
                        if (operatorSort == operators[i])
                            attempt = true;
                    }

                } while (!attempt);

                attempt = false;

                PropertyInfo property1 = typeof(Product).GetProperty(sortingBy);

                if (property1 != null)
                {
                    Func<Product, bool> filter = CreateFilter(property1, operatorSort, value);
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1.Condition to sorted list (All products in sorted list)\n2.Condition to main list (All products)");
                        string answer = Console.ReadLine();
                        switch (answer)
                        {
                            case "1":
                                attempt = true;
                                if (!copyList.Any())
                                {
                                    sortList = products.Where(filter).ToList();
                                }
                                else
                                {
                                    sortList = sortList.Where(filter).ToList();
                                }
                                sortList = sortList.Distinct().ToList();
                                break;
                            case "2":
                                copyList = products.Where(filter).ToList();
                                sortList = sortList.Concat(copyList).ToList();
                                attempt = true;
                                break;
                            default:
                                break;
                        }

                    } while (!attempt);
                    attempt = false;
                }
                bool endOfReport = false;

                do
                {
                    string report = "";
                    if(sortList.Count == 0)
                    {
                        Console.WriteLine("List is empty");
                    }
                    else
                    {
                        Console.Clear();
                        foreach (var product in sortList)
                        {
                            report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.date}\n- - - - - - - - - - -\n";
                            Console.WriteLine($"Name: {product.Name}");
                            Console.WriteLine($"Price: {product.Price}");
                            Console.WriteLine($"Quantity: {product.Quantity}");
                            Console.WriteLine($"Id: {product.Id}");
                            Console.WriteLine($"Date: {product.date}");
                            Console.WriteLine("- - - - - - - - - - - -");
                        }
                    }
                    Console.Write("\n\n1.Report to txt\n2.Antoher term condition\n3.Exit\nNumber: ");
                    string answer = Console.ReadLine();
                    switch(answer)
                    {
                        case "1":
                            ReportMenu(systemOp, report, ref endOfReport, sortList);
                            endOfSort = true;
                            break;
                        case "2":
                            endOfReport = true;
                            break;
                        case "3":
                            endOfReport = true;
                            endOfSort = true;
                            break;
                        default:
                            break;
                    }
                } while (!endOfReport);


            } while (!endOfSort); 
        }
        private static void ReportMenu(string systemOp, string report, ref bool endOfReport, List<Product> reportProducts)
        {
            Console.WriteLine("1.Record to txt file\n2.Record to pdf\n3.Record to excel\n4.Exit\n");

            Console.Write($"Number: ");
            string answer = Console.ReadLine();

            switch (answer)
            {
                case "1":
                    RecordingTxtFile(systemOp, report);
                    break;
                case "2":
                    PdfCreater(report);
                    break;
                case "3":
                    ExcelCreater(reportProducts);
                    break;
                case "4":
                    endOfReport = true;
                    break;

                default:
                    break;
            }
        }
        private static Func<Product, bool> CreateFilter(PropertyInfo property, string filter, string value)
        {
            var parameter = Expression.Parameter(typeof(Product), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var convertedFilterValue = Expression.Constant(Convert.ChangeType(value, property.PropertyType));
            var comparison = GetComparisonExpression(propertyAccess,filter, convertedFilterValue);
            return Expression.Lambda<Func<Product, bool>>(comparison, parameter).Compile();
        }
        private static Expression GetComparisonExpression(Expression left, string filter, Expression right)
        {
            switch(filter)
            {
                case ">":
                    return Expression.GreaterThan(left, right);
                case "<":
                    return Expression.LessThan(left, right);
                case "=":
                    return Expression.Equal(left, right);
                case "!=":
                    return Expression.NotEqual(left, right);
                case "<=":
                    return Expression.LessThanOrEqual(left, right);
                case ">=":
                    return Expression.GreaterThanOrEqual(left, right);
                default:
                    throw new FormatException("Critical Error");
            }
        }
        private static void RecordingTxtFile(string systemOp, string report)
        {
            if (!string.IsNullOrEmpty(report))
            {
                string fileName = Utils.NameFile();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                path = Path.Combine(path, "Desktop", fileName + ".txt");
                File.WriteAllText(path, report);
                Console.WriteLine("File is complete!");
            }
            else
            {
                Console.WriteLine("File is empty!\nClick enter to continue");
                Console.ReadKey();
            }
        }
        private static void PdfCreater(string report)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if(!string.IsNullOrEmpty(report))
            {
                string fileName = Utils.NameFile();

                string[] text = report.Split('\n');

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 12,XFontStyle.Regular);
                int y = 30;
                int x = 10;
                int lineHeight = 24;

                foreach (var item in text)
                {
                    if(y+lineHeight > page.Height.Point - 25)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = 30;
                    }
                    gfx.DrawString(item, font, XBrushes.Black, x, y);
                    y += lineHeight;
                }
                document.Save(Path.Combine(path,"Desktop", fileName+".pdf"));
            }
            else
            {
                Console.WriteLine("File is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            Console.Clear();
        }
        private static void ExcelCreater(List<Product> products)
        {
            if(products.Count == 0)
            {
                Console.WriteLine("List is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            else
            {
                string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                systemOp = Path.Combine(systemOp, "Desktop");

                string fileName = Utils.NameFile();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(new FileInfo(Path.Combine(systemOp, fileName + ".xlsx"))))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
                    worksheet.Cells[1, 1].Value = "Number";
                    worksheet.Cells[1, 2].Value = "Produt Name";
                    worksheet.Cells[1, 3].Value = "Id";
                    worksheet.Cells[1, 4].Value = "Price";
                    worksheet.Cells[1, 5].Value = "Quantity";
                    worksheet.Cells[1, 6].Value = "Date";
                    worksheet.Cells[1, 6].Style.Numberformat.Format = "yyyy-mm-dd";

                    var range = worksheet.Cells["A1:F1"];
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(OfficeOpenXml.Style.ExcelIndexedColor.Indexed10);
                    range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(OfficeOpenXml.Drawing.eThemeSchemeColor.Accent3);

                    for (int i = 0; i < products.Count; i++)
                    {
                        worksheet.Cells[i + 3, 1].Value = i + 1 + ".";
                        worksheet.Cells[i + 3, 2].Value = products[i].Name;
                        worksheet.Cells[i + 3, 3].Value = products[i].Id;
                        worksheet.Cells[i + 3, 4].Value = products[i].Price;
                        worksheet.Cells[i + 3, 5].Value = products[i].Quantity;
                        worksheet.Cells[i + 3, 6].Style.Numberformat.Format = "yyyy-mm-dd";
                        worksheet.Cells[i + 3, 6].Value = products[i].date;
                    }

                    worksheet.Cells.AutoFitColumns();
                    package.Save();
                }

            }
        }
    }
}

