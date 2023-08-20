using System;
namespace Warehouse_Application
{
    public class HistoryModifications
    {
        public ProductHistory before { get; set; }
        public ProductHistory after { get; set; }
        public DateTime date { get; set; }
        public Employee modifiedBy { get; set; }
        public string idModofication = "";


        public HistoryModifications(ProductHistory before, ProductHistory after, DateTime date,Employee modifiedBy)
        {
            this.date = date;
            this.before = before;
            this.after = after;
            this.modifiedBy = modifiedBy;
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0987654321";
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                idModofication += characters[random.Next(characters.Length)]; ///sprawdzenie czy danego id nie ma powtorzonego
            }
        }
        public HistoryModifications() { }
    }
}


