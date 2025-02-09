﻿using System.Text.RegularExpressions;
using System.Globalization;
namespace Warehouse_Application;
public class Product
{
    public List<HistoryModifications> listOfModifications = new List<HistoryModifications>();
    private string name;
    private string id;
    private double price;
    private int quantity;
    private DateTime date { get; set; }
    public int day { get; set; }
    public int year { get; set; } /// No idea to a better sorting method 
    public int month { get; set; }
    public Employee addedBy { get; set; }


    public Product(Product p1)
    {
        this.name = p1.name;
        this.id = p1.id;
        this.price = p1.price;
        this.quantity = p1.quantity;
        this.date = p1.date;
        this.listOfModifications = p1.listOfModifications;
        this.addedBy = p1.addedBy;
    }
    public Product(ProductHistory p1, List<HistoryModifications> list)
    {
        this.name = p1.name;
        this.id = p1.id;
        this.price = p1.price;
        this.quantity = p1.quantity;
        this.date = p1.date;
        this.listOfModifications = list.ToList();
        this.addedBy = p1.addedBy;
        
    }
    public Product(ProductHistory p1)
    {
        this.name = p1.name;
        this.id = p1.id;
        this.price = p1.price;
        this.quantity = p1.quantity;
        this.date = p1.date;
        this.addedBy = p1.addedBy;
    }
    public Product()
    {
        name = "";
        id = "";
        price = 0;
        quantity = 0;
        DateTime dateCopy = DateTime.Now;
        date = dateCopy.Date;
        addedBy = null;

        day = date.Day;
        month = date.Month;
        year = date.Year;
    }
    public Product(string name, string id, double price, int quantity, DateTime date, Employee employee)
    {
        if (Regex.IsMatch((id.Trim()), @"^[A-Za-z]{4}\d{5}$"))
        {
            this.id = id.Trim();
        }
        else
            throw new FormatException("Id is not correct");

        if (price > 0)
        {
            this.price = price;
        }
        else
            throw new FormatException("Price is not correct");

        if (quantity >= 0)
        {
            this.quantity = quantity;
        }
        else
            throw new FormatException("Quantity is not correct");

        if(Regex.IsMatch(name.Trim() , @"^[A-Za-z]{1}[A-Za-z0-9\s]+$") && name.Length > 0)
        {
            this.name = name.Trim();
        }
        else
            throw new FormatException("Name is not correct");

        this.addedBy = employee;
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
            if (value.Length > 0 && Regex.IsMatch((value), @"^[A-Za-z]{1}[A-Za-z0-9\s]+$"))
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
        listOfModifications.Add(p1);
    }
    public void ObjectGraphic()
    {
        Console.WriteLine($"1.Name:     {Name}");
        Console.WriteLine($"2.Price:    {Price}");
        Console.WriteLine($"3.Quantity: {Quantity}");
        Console.WriteLine($"4.Id:       {Id}");
        Console.WriteLine($"5.Date:     {Date}");
        Console.WriteLine($"6.Added by: {addedBy.Position} {addedBy.Name} {addedBy.LastName}\n");
    }

}


