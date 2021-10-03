namespace Core.Entities.OrderAggregate
{
    //this is going to act as a snapshot of order at the time or product item at the time
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int id, string name, string pictureUrl)
        {
            Id = id;
            Name = name;
            PictureUrl = pictureUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
    }
}