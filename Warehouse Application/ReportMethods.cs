using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Packaging.Ionic.Zlib;


namespace Warehouse_Application
{
    public static class ReportMethods
    {
        public static void ReportOfProducts(ref List<Product> products)
        {
            string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            bool endOfRaport = false;
            if (string.IsNullOrEmpty(File.ReadAllText(Path.Combine(systemOp, "WareHouse", "Products.json"))))
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
                        AllProductReport(products);
                        break;
                    case "2":
                        SearchingById(products);
                        break;
                    case "3":
                        SortingByValue(products);
                        break;
                    case "4":
                        endOfRaport = true;
                        break;
                    default:
                        break;

                }
            } while (!endOfRaport);
        }// Choosing to sort or sreach by id or to see all products
        private static void AllProductReport(List<Product> products)
        {
            bool endOfReport = false;
            do
            {
                string report = "";
                Console.WriteLine("REPORTS\n\n");
                Console.WriteLine("".PadLeft(40));
                foreach (var product in products)
                {
                    report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.Date}\nAdded by: {product.addedBy.Position} {product.addedBy.Name} {product.addedBy.LastName}\n - - - - - - - - \n";
                    Console.WriteLine($"Name:     {product.Name}");
                    Console.WriteLine($"Price:    {product.Price}");
                    Console.WriteLine($"Quantity: {product.Quantity}");
                    Console.WriteLine($"Id:       {product.Id}");
                    Console.WriteLine($"Date:     {product.Date}");
                    Console.WriteLine($"Added by: {product.addedBy.Position} {product.addedBy.Name} {product.addedBy.LastName}");
                    Console.WriteLine("".PadLeft(40));
                }
                Console.WriteLine("\n\n");
                ReportMenu(report, ref endOfReport, products);
            } while (!endOfReport);

        }// Show all of products
        private static void SearchingById(List<Product> products)
        {

            bool endSearching = false;
            do
            {
                Console.Clear();
                List<Product> copyList = products.ToList();
                Console.Write("Id (Write 0 to exit): ");
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
                            report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.Date}\nAdded by: {product.addedBy.Position} {product.addedBy.Name} {product.addedBy.LastName}\n - - - - - - - - \n";
                            Console.WriteLine("Name:     {0}" ,product.Name);
                            Console.WriteLine("Price:    {0}" ,product.Price);
                            Console.WriteLine("Quantity: {0}" ,product.Quantity);
                            Console.WriteLine("Id:       {0}" ,product.Id);
                            Console.WriteLine("Date:     {0}", product.Date);
                            Console.WriteLine("Added by: {0} {1} {2}",product.addedBy.Position, product.addedBy.Name, product.addedBy.LastName);
                        }
                        Console.WriteLine("\n\n");
                        ReportMenu(report, ref endSearching, copyList);

                    }
                    else
                    {
                        Console.WriteLine("\nId is not in the database\nClick enter to continue or 0 to exit");
                        string exitOrNot = Console.ReadLine();
                        if (exitOrNot == "0")
                            endSearching = true;
                    }
                }
                else if (idSearching == "0")
                    endSearching = true;
                else
                {
                    Console.WriteLine("\nWrong id (4 Letters and 5 numbers, example: Abcd12345)\nClick enter to continue or 0 to exit");
                    string exitOrNot = Console.ReadLine();
                }

            } while (!endSearching);
        } // show product with inscribed id
        //tutaj
        private static void SortingByValue(List<Product> products)
        {
            List<Product> copyList = new List<Product>();
            List<Product> sortList = new List<Product>();
            bool endOfSort = false, attempt = true, itIsString = true;
            string sortingBy, operatorSort = "";
            DateTime dateSorting;
            string value = "";
            int year, month, day;
            do
            {
                itIsString = true;
                do
                {
                    attempt = true;
                    Console.Clear();
                    Console.Write("1.Sort by value price\n2.Sort by value quantity\n3.Sort by date\n4.Sort by employee\n5.Exit\n\nNumber: ");
                    sortingBy = Console.ReadLine();
                    switch (sortingBy)
                    {
                        case "1":
                            sortingBy = "Price";
                            itIsString = false;
                            break;
                        case "2":
                            sortingBy = "Quantity";
                            itIsString = false;
                            break;
                        case "3":
                            sortingBy = "date";
                            itIsString = false;
                            break;
                        case "4":
                            sortingBy = "addedBy";
                            do
                            {
                                attempt = true;
                                Console.Clear();
                                Console.Write("Sorting by:\n1.Position\n2.Name\n3.Last name\n4.Age\n5.Id Employee\nNumber: ");
                                string answer = Console.ReadLine();
                                switch (answer)
                                {
                                    case "1":
                                        sortingBy += ".Position";
                                        break;
                                    case "2":
                                        sortingBy += ".name";
                                        break;
                                    case "3":
                                        sortingBy += ".LastName";
                                        break;
                                    case "4":
                                        sortingBy += ".Age";
                                        itIsString = false; 
                                        break;
                                    case "5":
                                        sortingBy += ".Id";
                                        break;
                                    default:
                                        attempt = false;
                                        break;
                                }
                            } while (!attempt);
                            break;
                        case "5":
                            return;
                        default:
                            attempt = false;
                            break;
                    }

                } while (!attempt);

                attempt = false;
                do
                {
                    Console.Clear();
                    if (sortingBy == "date")
                    {
                        bool yearBool, monthBool, dayBool;
                        Console.Clear();
                        Console.Write("Sorting by:\n1. Day\n2. Month\n3. Year\n4. Date\n\nNumber: ");
                        string answerDate = Console.ReadLine();
                        switch (answerDate)
                        {
                            case "1":
                                sortingBy = "day";
                                Console.Write("Day: ");
                                dayBool = int.TryParse(Console.ReadLine(), out day);
                                if (!dayBool)
                                {
                                    break;
                                }
                                value = day.ToString();
                                attempt = true;
                                break;

                            case "2":
                                sortingBy = "month";
                                Console.Write("Month: ");
                                monthBool = int.TryParse(Console.ReadLine(), out month);
                                if (!monthBool)
                                {
                                    break;
                                }
                                value = month.ToString();
                                attempt = true;
                                break;

                            case "3":
                                sortingBy = "year";
                                Console.Write("Year: ");
                                yearBool = int.TryParse(Console.ReadLine(), out year);
                                if (!yearBool)
                                {
                                    break;
                                }
                                value = year.ToString();
                                attempt = true;
                                break;

                            case "4":
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
                                            dateSorting = new DateTime(year, month, day);
                                            value = dateSorting.ToString();
                                            attempt = true;

                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if(sortingBy == "addedBy.Position")
                    {
                        bool addedByAccept = true;
                        do
                        {
                            addedByAccept = true;
                            Console.Clear();
                            Console.Write("1.Admin\n2.Manager\n3.Employee\n4.Supplier\nNumber: ");
                            string answer = Console.ReadLine();
                            switch(answer)
                            {
                                case "1":
                                    value = PositionName.Admin.ToString();
                                    break;
                                case "2":
                                    value = PositionName.Manager.ToString();
                                    break;
                                case "3":
                                    value = PositionName.Employee.ToString();
                                    break;
                                case "4":
                                    value = PositionName.Supplier.ToString();
                                    break;
                                default:
                                    addedByAccept = false;
                                    break;
                            }

                        } while (!addedByAccept);
                        attempt = true;
                    }
                    else if(itIsString)
                    {
                        Console.Clear();
                        Console.Write("String: ");
                        value = Console.ReadLine();
                        if (value.Length > 0)
                            attempt = true;
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
                if(itIsString)
                {
                    do
                    {
                        Console.Clear();
                        Console.Write($"Choose one of this operators ( = , != )\n\n{sortingBy} [operator] {value}: ");
                        operatorSort = Console.ReadLine();
                        string[] operators = new string[] { "=", "!="};
                        for (int i = 0; i < operators.Length; i++)
                        {
                            if (operatorSort == operators[i])
                                attempt = true;
                        }

                    } while (!attempt);
                }
                else
                {
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
                }

                if (sortingBy.Split('.')[0] == "addedBy")
                {
                    Func<Product, bool> filter = FilterAndSortByAddedBy(sortingBy, value, operatorSort,itIsString);

                    do
                    {
                        Console.Clear();
                        Console.Write("1. Condition to sorted list (All products in sorted list)\n2. Condition to main list (All products)\n\nNumber: ");
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
                                break;
                            case "2":
                                copyList = products.Where(filter).Distinct().ToList();
                                sortList = sortList.Concat(copyList).Distinct().ToList();
                                attempt = true;
                                break;
                            default:
                                break;
                        }

                    } while (!attempt);

                }
                else
                {

                    PropertyInfo property1 = typeof(Product).GetProperty(sortingBy);

                    if (property1 != null)
                    {
                        Func<Product, bool> filter = CreateFilter(property1, operatorSort, value, itIsString);
                        do
                        {
                            Console.Clear();
                            Console.Write("1. Condition to sorted list (All products in sorted list)\n2. Condition to main list (All products)\n\nNumber: ");
                            string answer = Console.ReadLine();
                            switch (answer)
                            {
                                case "1":
                                    attempt = true;
                                    if (!copyList.Any())
                                    {
                                        sortList = products.Where(filter).Distinct().ToList();
                                    }
                                    else
                                    {
                                        sortList = sortList.Where(filter).Distinct().ToList();
                                    }
                                    break;
                                case "2":
                                    copyList = products.Where(filter).Distinct().ToList();
                                    sortList = sortList.Concat(copyList).Distinct().ToList();
                                    attempt = true;
                                    break;
                                default:
                                    break;
                            }

                        } while (!attempt);
                        attempt = false;
                    }

                }
                bool endOfReport = false;

                do
                {
                    string report = "";
                    if (sortList.Count == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("List is empty");
                    }
                    else
                    {
                        Console.Clear();
                        foreach (var product in sortList)
                        {
                            report += $"Name: {product.Name}\nPrice: {product.Price}\nQuantity: {product.Quantity}\nId: {product.Id}\nDate: {product.Date}\n- - - - - - - - - - -\n";
                            Console.WriteLine($"Name: {product.Name}");
                            Console.WriteLine($"Price: {product.Price}");
                            Console.WriteLine($"Quantity: {product.Quantity}");
                            Console.WriteLine($"Id: {product.Id}");
                            Console.WriteLine($"Date: {product.Date}");
                            Console.WriteLine($"Added by: {product.addedBy.Position} {product.addedBy.Name} {product.addedBy.LastName}");
                            Console.WriteLine("- - - - - - - - - - - -");
                        }
                    }
                    Console.Write("\n\n1.Report\n2.Antoher term condition\n3.Exit\nNumber: ");
                    string answer = Console.ReadLine();
                    switch (answer)
                    {
                        case "1":
                            Console.WriteLine();
                            ReportMenu(report, ref endOfReport, sortList);
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
        } // show products with range of values
        private static void ReportMenu(string report, ref bool endOfReport, List<Product> reportProducts)
        {
            Console.WriteLine("1.Write down to txt file\n2.Write down to pdf\n3.Write down to excel\n4.Statistics of list\n5.Exit\n");

            Console.Write($"Number: ");
            string answer = Console.ReadLine();

            switch (answer)
            {
                case "1":
                    TxtFile(report);
                    break;
                case "2":
                    PdfCreater(report);
                    break;
                case "3":
                    ExcelCreater(reportProducts);
                    break;
                case "4":
                    Utils.Statistics(reportProducts);
                    break;
                case "5":
                    endOfReport = true;
                    break;

                default:
                    break;
            }
        } // menu with choosing report (txt,pdf,excel)
        private static Func<Product, bool> CreateFilter(PropertyInfo property, string filter, string value,bool isItString)
        {
            var parameter = Expression.Parameter(typeof(Product), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var convertedFilterValue = Expression.Constant(Convert.ChangeType(value, property.PropertyType));
            var comparison = GetComparisonExpression(propertyAccess, filter, convertedFilterValue,isItString);
            return Expression.Lambda<Func<Product, bool>>(comparison, parameter).Compile();
        } // creating an expression Lambda with comaprison (Object with object property)
        private static Func<Product,bool> FilterAndSortByAddedBy(string propertyName, string value,string filter,bool isItString)
        {
           
            var param = Expression.Parameter(typeof(Product), "x");
            Expression property = propertyName.Split('.').Aggregate((Expression)param, Expression.PropertyOrField);
            if(property.Type.IsEnum)
            {
                var enumValue = Enum.Parse(property.Type, value);
                var converted = Expression.Constant(enumValue);
                var comparison = GetComparisonExpression(property, filter, converted, isItString);
                return Expression.Lambda<Func<Product, bool>>(comparison, param).Compile();
            }
            else
            {
                var convertedFilterValue = Expression.Constant(Convert.ChangeType(value, property.Type));
                var comparison = GetComparisonExpression(property, filter, convertedFilterValue, isItString);
                return Expression.Lambda<Func<Product, bool>>(comparison, param).Compile();
            }

        } // creating an expression Lamda with comaprison
        private static Expression GetComparisonExpression(Expression left, string filter, Expression right, bool isItString)
        {

            if(isItString) 
            {
                switch (filter)
                {
                    case "=":
                        return Expression.Equal(left ,right);
                    case "!=":
                        return Expression.NotEqual(left,right);
                    default:
                        throw new FormatException("Critical Error");
                }
            }
            else
            {
                switch (filter)
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
            
        } // Comaprison to method "CreateFilter"
        private static void TxtFile(string report)
        {
            if (!string.IsNullOrEmpty(report))
            {
                string fileName = Utils.NameFile();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                path = Path.Combine(path, "WareHouse", fileName + ".txt");
                File.WriteAllText(path, report);
                Console.WriteLine("File is complete!");
            }
            else
            {
                Console.WriteLine("File is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            Console.Clear();
        } // Saving to txt file
        private static void PdfCreater(string report)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (!string.IsNullOrEmpty(report))
            {
                string fileName = Utils.NameFile();

                string[] text = report.Split('\n');

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 12, XFontStyle.Regular);
                int y = 30;
                int x = 10;
                int lineHeight = 24;

                foreach (var item in text)
                {
                    if (y + lineHeight > page.Height.Point - 25)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = 30;
                    }
                    gfx.DrawString(item, font, XBrushes.Black, x, y);
                    y += lineHeight;
                }
                document.Save(Path.Combine(path, "WareHouse", fileName + ".pdf"));
            }
            else
            {
                Console.WriteLine("File is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            Console.Clear();
        } // Saving to pdf file
        private static void ExcelCreater(List<Product> products)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("List is empty!\nClick enter to continue");
                Console.ReadKey();
            }
            else
            {
                string systemOp = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string fileName = Utils.NameFile();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(new FileInfo(Path.Combine(systemOp, "WareHouse", fileName + ".xlsx"))))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
                    worksheet.Cells[1, 1].Value = "Number";
                    worksheet.Cells[1, 2].Value = "Produt Name";
                    worksheet.Cells[1, 3].Value = "Id";
                    worksheet.Cells[1, 4].Value = "Price";
                    worksheet.Cells[1, 5].Value = "Quantity";
                    worksheet.Cells[1, 6].Value = "Date";
                    worksheet.Cells[1, 7].Value = "Added by";
                    worksheet.Cells[1, 6].Style.Numberformat.Format = "yyyy-mm-dd";

                    var range = worksheet.Cells["A1:G1"];
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
                        worksheet.Cells[i + 3, 6].Value = products[i].Date;
                        worksheet.Cells[i + 3, 7].Value = products[i].addedBy.Position + " " + products[i].addedBy.LastName + "" + products[i].addedBy.Name;
                    }

                    worksheet.Cells.AutoFitColumns();
                    package.Save();
                    Console.Clear();
                }

            }
        }// Saving to excel file
    }
}

