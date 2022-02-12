namespace Core
{
    public class ProductAddDb
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public int ProductTypeId { get; set; }
        public int SystemTypeId { get; set; }
    }
}