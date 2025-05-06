namespace CleanArchitectureTemp.Domain.DomainShared.Contracts.Repositories;

public interface IRepository
{
    /// <summary>
    /// Initialize the database context.
    /// </summary>
    /// <returns></returns>
    Task InitDbContextAsync();
}