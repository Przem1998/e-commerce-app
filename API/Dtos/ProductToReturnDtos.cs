namespace API.Dtos
{
    public class ProductToReturnDtos // DTO contains only getters and setters
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public string Description { get; set; }
       public decimal Price { get; set; }
       public string PictureUrl { get; set; }
       public string ProductType { get; set; }
       public string ProductSize { get; set; }
    }
}