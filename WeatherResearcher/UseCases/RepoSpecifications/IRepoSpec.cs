using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.RepoSpecifications
{

    public interface ICriteriaSpec<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
    }


    public interface IIncludesSpec<T>
    {
        List<Expression<Func<T, object>>> Includes { get; }
    }
}