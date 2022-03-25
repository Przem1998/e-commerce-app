using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productParams)
        :base (x=>(
                  (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search))&&
                  (!productParams.SystemId.HasValue || x.SystemTypeId==productParams.SystemId)&&
                  (!productParams.TypeId.HasValue || x.ProductTypeId==productParams.TypeId))
            )   
        {

        }
    }
}