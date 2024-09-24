using ASPNETCoreWithHeadersMiddleware.Entities;
using ASPNETCoreWithHeadersMiddleware.Handlers.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace ASPNETCoreWithHeadersMiddleware.Handlers
{
    internal class RedisPostHandler : IRedisHandler<PostEntity, string>, IDisposable, IAsyncDisposable
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisPostHandler(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            return await _db.KeyDeleteAsync(id);
        }

        public async Task<PostEntity> GetByIdAsync(string id)
        {
            var entityJson = await _db.StringGetAsync(id);
            return !string.IsNullOrEmpty(entityJson) ? JsonSerializer.Deserialize<PostEntity>(entityJson) : null;
        }

        public async Task<bool> IsExistByIdAsync(string id)
        {
            var entityJson = await _db.StringGetAsync(id);
            return !string.IsNullOrEmpty(entityJson);
        }

        public Task<bool> SaveAsync(PostEntity entity)
        {
            return _db.StringSetAsync(entity.Id, JsonSerializer.Serialize(entity));
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_redis != null) await _redis.DisposeAsync();
        }
    }
}
