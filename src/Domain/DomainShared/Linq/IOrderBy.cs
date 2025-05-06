using System.Linq.Expressions;

namespace CleanArchitectureTemp.Domain.DomainShared.Linq;

public interface IOrderBy<TSource, TKey>
{
    public bool IsDescending { get; }
    /// <summary>
    /// Order by expression
    /// </summary>
    Expression<Func<TSource, TKey>> Expression { get; }
}