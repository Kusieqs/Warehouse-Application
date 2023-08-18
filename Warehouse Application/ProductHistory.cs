using System;
namespace Warehouse_Application
{
	public class ProductHistory
	{
        public string name { get; set; }
        public string id { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public DateTime date { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public Employee addedBy { get; set; }
        public ProductHistory(Product p1)
		{
            name = p1.Name;
            id = p1.Id;
            price = p1.Price;
            quantity = p1.Quantity;
            this.date = p1.Date;
            day = p1.day;
            month = p1.month;
            year = p1.year;
            this.addedBy = p1.addedBy;
		}
        public ProductHistory()
        { }
	}
}

