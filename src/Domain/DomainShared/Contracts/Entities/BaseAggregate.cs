
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitectureTemp.Domain.DomainShared.Contracts.Entities;

public interface IAggregateKey<TKey> 
    where TKey : struct
{
    static abstract TKey NewKey();
}

public readonly struct IntKey : IAggregateKey<int>
{
    public static int NewKey() => default; 
}

public readonly struct GuidKey : IAggregateKey<Guid>
{
    public static Guid NewKey() => Guid.NewGuid();
}


public abstract class BaseAggregate<TKey> where TKey : struct, IAggregateKey<TKey>
{
    public required TKey Id {get ;set;} = TKey.NewKey();

    [NotMapped]
    private readonly List<BaseEvent> _domainEvents = new();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

}