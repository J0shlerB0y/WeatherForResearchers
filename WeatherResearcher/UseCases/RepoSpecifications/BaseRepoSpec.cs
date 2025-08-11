using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.RepoSpecifications
{
    public abstract class BaseCriteriaSpec<T> : ICriteriaSpec<T>
    {
        public Expression<Func<T, bool>>? Criteria { get; private set; }

        protected BaseCriteriaSpec(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected BaseCriteriaSpec() { }
    }


    public abstract class BaseIncludesSpec<T> : IIncludesSpec<T>
    {
        public List<Expression<Func<T, object>>> Includes { get; } = new();

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }

}
