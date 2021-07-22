namespace Core.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

       public decimal Price { get; set; }
       public string PictureUrl { get; set; }

       public string ProductType { get; set; }

       
       public string ProductSize { get; set; }
    }
}