using ASPNETCoreWithHeadersMiddleware.Entities;
using ASPNETCoreWithHeadersMiddleware.Handlers.Interfaces;
using ASPNETCoreWithHeadersMiddleware.Services.Interfaces;

namespace ASPNETCoreWithHeadersMiddleware.Services
{
    internal class PostService : IPostService
    {
        private readonly IRedisHandler<PostEntity, string> _redisHandler;

        public PostService(IRedisHandler<PostEntity, string> redisHandler)
        {
            _redisHandler = redisHandler;
        }

        public async Task CreateAsync(PostEntity post)
        {
            await _redisHandler.SaveAsync(post);
        }

        public async Task<PostEntity> GetAsync(string id)
        {
            return await _redisHandler.GetByIdAsync(id);
        }

        public async Task UpdateAsync(PostEntity post)
        {
            await _redisHandler.SaveAsync(post);
        }

        public async Task DeleteAsync(string id)
        {
            await _redisHandler.DeleteByIdAsync(id);
        }

        public async Task<bool> IsExistAsync(string id)
        {
            return await _redisHandler.IsExistByIdAsync(id);
        }
    }
}
