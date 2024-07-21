using StackExchange.Redis;
using System.Text.Json;

namespace RedisAPI2.Services
{
	public class RedisService
	{
		private readonly IConnectionMultiplexer _redis;
		private readonly IDatabase _database;

		public RedisService(IConnectionMultiplexer redis)
		{
			_redis = redis;
			_database = _redis.GetDatabase();
		}

		public async Task SetCacheAsync<T>(string key, T value)
		{
			var json = JsonSerializer.Serialize(value);
			await _database.StringSetAsync(key, json);
		}

		public async Task<T> GetCacheAsync<T>(string key)
		{
			var json = await _database.StringGetAsync(key);
			return json.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(json);
		}
	}
}