namespace Stock.Application.Services.Interfaces
{
    public interface IServicesBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T dto);
        Task<T> UpdateAsync(int id, T dto);
        Task<bool> DeleteAsync(int id);
    }
}
