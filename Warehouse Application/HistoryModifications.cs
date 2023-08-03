using System;
namespace Warehouse_Application
{
	public class HistoryModifications
	{
		public Product p1 { get; set; }
		public Product p2 { get; set; }
		public DateTime date { get; set; }
		public HistoryModifications(Product p1, Product p2, DateTime date)
		{
			this.date = date;
			this.p1 = p1;
			this.p2 = p2;
		}
	}
}

