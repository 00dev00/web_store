using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext storeContext) : IGenericRepository<T> where T : BaseEntity
{
    private StoreContext _context { get; } = storeContext;

    public async Task<T> Get(int id)
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec) 
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
    }
}