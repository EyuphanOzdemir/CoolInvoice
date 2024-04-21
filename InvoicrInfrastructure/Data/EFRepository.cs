using Ardalis.Specification.EntityFrameworkCore;

namespace InvoicrInfrastructure.Data
{
  public class EFRepository<T>(AppDbContext dbContext) : RepositoryBase<T>(dbContext) where T:class
  {
  }
}
