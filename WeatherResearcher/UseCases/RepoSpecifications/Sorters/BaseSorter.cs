using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.RepoSpecifications.Sorters
{
    public class BaseSorter<T> : ISorterSpec<T>
    {
        public Expression<Func<T, object>>? OrderBy { get; private set; }
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        public BaseSorter(SortingEnum sortingState, Dictionary<SortingEnum, Expression<Func<T, object>>> sortingExpression)
        {
            bool isDescending = sortingState.ToString().Contains("Desc");

            SortingEnum baseSortKey = sortingState;
            if (isDescending)
            {
                if (!Enum.TryParse(sortingState.ToString().Replace("Desc", "Asc"), out baseSortKey))
                {
                    throw new Exception("Failure parse Desc, but is Desc");
                }
            }

            if (sortingExpression.TryGetValue(baseSortKey, out var sortExpression))
            {
                if (isDescending)
                {
                    AddOrderByDescending(sortExpression);
                }
                else
                {
                    AddOrderBy(sortExpression);
                }

            }
        }
    }
}
