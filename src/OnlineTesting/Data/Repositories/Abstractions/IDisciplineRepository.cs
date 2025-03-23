using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories.Abstractions;

public interface IDisciplineRepository : IGenericRepository<Discipline>
{
    Task<Discipline> GetByNameAsync(string name);
}