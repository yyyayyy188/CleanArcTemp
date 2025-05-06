using System.Linq.Expressions;
using CleanArchitectureTemp.Domain.DomainShared.Contracts.Entities;

namespace CleanArchitectureTemp.Domain.DomainShared.Linq;

public class OrderBy<TSource, TKey> : IOrderBy<TSource, TKey>
{
    public OrderBy(Expression<Func<TSource, TKey>> expression, bool isDescending = false)
    {
        Expression = expression;
        IsDescending = isDescending;
    }

    public Expression<Func<TSource, TKey>> Expression { get; }
    public bool IsDescending { get; }
}