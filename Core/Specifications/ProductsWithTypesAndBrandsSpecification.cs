using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification()
        {
          AddInclude(x=> x.ProductType);
          AddInclude(x=> x.ProductBrand);
        }
      
        // add criteria for Brand and Type
        
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams) : 
        base (x=>(
                  (!productParams.BrandId.HasValue || x.ProductBrandId==productParams.BrandId)&&
                  (!productParams.TypeId.HasValue || x.ProductTypeId==productParams.TypeId))
                )
      {
          AddInclude(x=> x.ProductType);
          AddInclude(x=> x.ProductBrand);
          AddOrderBy(x=> x.Name);
          ApplyPaging(productParams.PageSize+(productParams.PageIndex-1) ,productParams.PageSize);

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
        public ProductsWithTypesAndBrandsSpecification(int id) : base(x=>x.Id==id)
        {
          AddInclude(x=> x.ProductType);
          AddInclude(x=> x.ProductBrand);
        }
    }
}