using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemp.Infrastructure.EntityFrameworkCore;

public class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    public bool IsEnableTransaction { get; set; }
}
