using System;
namespace Warehouse_Application
{
    public class HistoryModifications
    {
        public ProductHistory before;
        public ProductHistory after;
        public DateTime date { get; set; }
        public string idModofication = "";


        public HistoryModifications(ProductHistory before, ProductHistory after, DateTime date)
        {
            this.date = date;
            this.before = before;
            this.after = after;
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


