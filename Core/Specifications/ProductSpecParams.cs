namespace Core.Specifications
{
    public class ProductSpecParams //Modeling display results and add access to init paramaters
    {
        private const int MaxPageSize=50;
        public int PageIndex { get; set; } =1; //default display first page
        
        private int _pageSize=6;

        public int PageSize 
        { 
         get=>_pageSize;
         set=> _pageSize=(value>MaxPageSize) ? MaxPageSize: value;
        }

        public int? SystemId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }

        private string _search;
        public string Search 
        { 
            get => _search;
            set => _search=value.ToLower();
        }  
    }
}