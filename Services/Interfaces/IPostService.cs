using ASPNETCoreWithHeadersMiddleware.Entities;

namespace ASPNETCoreWithHeadersMiddleware.Services.Interfaces
{
    public interface IPostService
    {
        Task CreateAsync(PostEntity post);
        Task<PostEntity> GetAsync(string id);
        Task UpdateAsync(PostEntity post);
        Task DeleteAsync(string id);

        Task<bool> IsExistAsync(string id);
    }
}
