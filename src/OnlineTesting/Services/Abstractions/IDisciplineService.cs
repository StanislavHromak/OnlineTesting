using OnlineTesting.Models;

namespace OnlineTesting.Services.Abstractions;

public interface IDisciplineService
{
    Task<Discipline> GetByIdAsync(int id);
    Task<IEnumerable<Discipline>> GetAllAsync();
    Task CreateAsync(Discipline discipline);
    Task UpdateAsync(Discipline discipline);
    Task DeleteAsync(int id);
}
