using System;
using System.Text.RegularExpressions;
namespace Warehouse_App
{
    public class Product
    {
        private string name;
        private string id;
        private double price;
        private int quantity;
        public DateTime date { get; set; }

        public Product(string name, string id, double price, int quantity, DateTime date)
        {
            if (Regex.IsMatch((id), @"^[A-Za-z]{4}\d{5}&") && price > 0 && quantity >= 0)
            {
                this.id = id.Trim();
                this.price = price;
                this.quantity = quantity;
            }
            else
                throw new FormatException("Informations about product are not correct");

            this.name = name.Trim();
            this.date = date;
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value.Trim();
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
                if (Regex.IsMatch((value), @"^[A-Za-z]{4}\d{5}&"))
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

    }
}


