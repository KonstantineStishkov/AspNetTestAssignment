namespace AspNetTestAssignment.Models
{
    public class History
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public DateTime OrderDate { get; set; }
        public string StoreCity { get; set; }

        public History() 
        {
            Id = Guid.NewGuid().ToString();
            OrderDate = DateTime.Now;
            StoreCity = string.Empty;
        }

        public History(string id, DateTime orderDate, string storeCity)
        {
            Id = id;
            OrderDate = orderDate;
            StoreCity = storeCity;
        }
    }
}
