using Microsoft.EntityFrameworkCore;
using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;

namespace OnlineTesting.Data.Repositories;

public class DisciplineRepository : GenericRepository<Discipline>, IDisciplineRepository
{
    public DisciplineRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Discipline> GetByNameAsync(string name)
    {
        return await _context.Disciplines.FirstOrDefaultAsync(d => d.Name == name);
    }
}
