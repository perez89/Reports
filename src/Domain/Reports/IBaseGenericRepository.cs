namespace Domain.Reports;

public interface IBaseGenericRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T> GetAsync(Guid id);
    public Task<T> CreateAsync(T Report);
    public T Update(T Report);
    public Task<bool> DeleteAsync(Guid id);
    public Task<int> SaveAsync();
}