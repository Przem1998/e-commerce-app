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
      
        public ProductsWithTypesAndBrandsSpecification(string sort)
        {
          AddInclude(x=> x.ProductType);
          AddInclude(x=> x.ProductBrand);
     

          if(!string.IsNullOrEmpty(sort))
          {
            switch(sort)
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