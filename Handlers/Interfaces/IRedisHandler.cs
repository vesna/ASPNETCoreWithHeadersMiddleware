namespace ASPNETCoreWithHeadersMiddleware.Handlers.Interfaces
{
    public interface IRedisHandler<T, in TKey> where T : class
    {
        Task<bool> DeleteByIdAsync(TKey id);
        Task<T> GetByIdAsync(TKey id);
        Task<bool> IsExistByIdAsync(TKey id);
        Task<bool> SaveAsync(T entity);
    }
}
