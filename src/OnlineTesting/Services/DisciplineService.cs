using OnlineTesting.Data.Repositories.Abstractions;
using OnlineTesting.Models;
using OnlineTesting.Services.Abstractions;

namespace OnlineTesting.Services;

public class DisciplineService : IDisciplineService
{
    private readonly IUnitOfWork _unitOfWork;

    public DisciplineService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Discipline> GetByIdAsync(int id)
    {
        return await _unitOfWork.Disciplines.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Discipline>> GetAllAsync()
    {
        return await _unitOfWork.Disciplines.GetAllAsync();
    }

    public async Task CreateAsync(Discipline discipline)
    {
        await _unitOfWork.Disciplines.AddAsync(discipline);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Discipline discipline)
    {
        _unitOfWork.Disciplines.Update(discipline);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var discipline = await _unitOfWork.Disciplines.GetByIdAsync(id);
        if (discipline != null)
        {
            _unitOfWork.Disciplines.Delete(discipline);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
