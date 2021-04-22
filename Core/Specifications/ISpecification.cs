using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T,bool>> Criteria{get;}
        List<Expression<Func<T,object>>> Includes{get;}  // include relationship in else case we do not have expressions from foreign key
        Expression<Func<T,object>> OrderBy {get;}  // sorting
        Expression<Func<T,object>> OrderByDesc {get;} //sorting

        //pagination
        int Take{get;}
        int Skip{get;}
        bool IsPaginEnabled{get;}

    }
}