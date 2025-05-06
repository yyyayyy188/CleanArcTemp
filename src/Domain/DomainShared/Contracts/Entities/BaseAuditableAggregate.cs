namespace CleanArchitectureTemp.Domain.DomainShared.Contracts.Entities;

public abstract class BaseAuditableAggregate<TKey> : BaseAggregate<TKey>
    where TKey : struct, IAggregateKey<TKey>
{
    public DateTime CreatedAt { get; private set; } 
    public DateTime? UpdatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public string? UpdatedBy { get; private set; }

    public void SetCreated(string createdBy)
    {
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetUpdated(string updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }
}