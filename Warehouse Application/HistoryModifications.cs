using System;
namespace Warehouse_Application
{
    public class HistoryModifications
    {
        public Product before { get; set; }
        public Product after { get; set; }
        public DateTime date { get; set; }
        public HistoryModifications(Product before, Product after, DateTime date)
        {
            this.date = date;
            this.before = before;
            this.after = after;
        }
        public HistoryModifications() { }
    }
}


