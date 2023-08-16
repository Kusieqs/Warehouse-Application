using System.Text.RegularExpressions;
namespace Warehouse_Application;
public class Product
{
    public List<HistoryModifications> list = new List<HistoryModifications>();
    private string name;
    private string id;
    private double price;
    private int quantity;
    private DateTime date { get; set; }
    public int day { get; set; }
    public int year { get; set; } /// No idea to a better sorting method 
    public int month { get; set; }
    private Employee addedBy;


    public Product(Product p1)
    {
        this.name = p1.name;
        this.id = p1.id;
        this.price = p1.price;
        this.quantity = p1.quantity;
        this.date = p1.date;
        this.list = p1.list;
    }
    public Product(ProductHistory p1, List<HistoryModifications> list)
    {
        this.name = p1.name;
        this.id = p1.id;
        this.price = p1.price;
        this.quantity = p1.quantity;
        this.date = p1.date;
        this.list = list.ToList();
    }
    public Product(ProductHistory p1)
    {
        this.name = p1.name;
        this.id = p1.id;
        this.price = p1.price;
        this.quantity = p1.quantity;
        this.date = p1.date;
    }
    public Product()
    {
        name = "";
        id = "";
        price = 0;
        quantity = 0;
        DateTime dateCopy = DateTime.Now;
        date = dateCopy.Date;
        
        day = date.Day;
        month = date.Month;
        year = date.Year;
    }
    public Product(string name, string id, double price, int quantity, DateTime date)
    {
        if (Regex.IsMatch((id.Trim()), @"^[A-Za-z]{4}\d{5}$") && price > 0 && quantity >= 0 && name.Length > 0)
        {
            this.id = id.Trim();
            this.price = price;
            this.quantity = quantity;
            this.name = name.Trim();
        }
        else
            throw new FormatException("Informations about product are not correct");

        this.date = date;
        day = date.Day;
        month = date.Month;
        year = date.Year;
    }
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            if (value.Length > 0)
            {
                name = value.Trim();
            }
            else
                throw new FormatException("Name is not correct");
        }
    }
    public string Id
    {
        get
        {
            return id;
        }
        set
        {
            if (Regex.IsMatch((value), @"^[A-Za-z]{4}\d{5}$"))
            {
                id = value.Trim();
            }
            else
                throw new FormatException("Id is not correct");
        }
    }
    public double Price
    {
        get
        {
            return price;
        }
        set
        {
            if (value > 0)
            {
                price = value;
            }
            else
                throw new FormatException("Price is not correct");
        }
    }
    public int Quantity
    {
        get
        {
            return quantity;
        }
        set
        {
            if (value >= 0)
            {
                quantity = value;
            }
            else
                throw new FormatException("Quantity is not correct");
        }
    }
    public DateTime Date
    {
        get
        {
            return date;
        }
        set
        {
            int daysInMonth = DateTime.DaysInMonth(value.Year, value.Month);
            if(daysInMonth>=value.Day && value.Month <=12 && value.Month>0 && value.Day > 0 && value.Year > 0)
            {
                date = value;
                day = value.Day;
                month = value.Month;
                year = value.Year;
            }
        }
    }
    public void HistoryOfProduct(HistoryModifications p1)
    {
        list.Add(p1);
    }
}


