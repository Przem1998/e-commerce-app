using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndSizesSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndSizesSpecification()
        {
          AddInclude(x=> x.ProductType);
          AddInclude(x=> x.SystemType);
        }
      
        // add criteria for Brand and Type
        
        public ProductsWithTypesAndSizesSpecification(ProductSpecParams productParams) : 
        base (x=>(
                  (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search))&&
                  (!productParams.SystemId.HasValue || x.SystemTypeId==productParams.SystemId)&&
                  (!productParams.TypeId.HasValue || x.ProductTypeId==productParams.TypeId))
                )
      {
          AddInclude(x=> x.ProductType);
          AddInclude(x=> x.SystemType);
          AddOrderBy(x=> x.Name);
          ApplyPaging(productParams.PageSize*(productParams.PageIndex-1) ,productParams.PageSize);

          if(!string.IsNullOrEmpty(productParams.Sort))
          {
            switch(productParams.Sort)
            {
              case "priceAsc":
                AddOrderBy(p=>p.Price);
                break;
            
              case "priceDesc":
                AddOrderByDesc(p=>p.Price);
                break;
            }
          }
        }
        public ProductsWithTypesAndSizesSpecification(int id) : base(x=>x.Id==id)
        {
          AddInclude(x=> x.ProductType);
          AddInclude(x=> x.SystemType);
        }
    }
}