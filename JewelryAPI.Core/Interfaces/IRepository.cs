namespace JewelryAPI.Core.Interfaces;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetQueryable();
    Task<T?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
