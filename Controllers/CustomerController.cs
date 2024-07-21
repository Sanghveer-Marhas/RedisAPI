using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RedisAPI2.Services;
using RedisTestAPI.Models;
using RedisTestAPI.Services;
using Serilog;
using System.Text.Json;

namespace RedisTestAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		private readonly CustomerService _customerService;
		private readonly RedisService _redisCacheService;

		public CustomerController(CustomerService customerService, RedisService redisCacheService)
		{
			_customerService = customerService;
			_redisCacheService = redisCacheService;
		}

		[HttpGet("/UserData")]
		public async Task<ActionResult<Customers>> GetUsersAsync()
		{
			try
			{

				var cachedUsersJson = await _redisCacheService.GetCacheAsync<string>("users");

				Log.Information(cachedUsersJson);

				if (!string.IsNullOrEmpty(cachedUsersJson))
				{
					var users = JsonSerializer.Deserialize<List<Customers>>(cachedUsersJson);
					return Ok(users);
				}

				var usersFromService = await _customerService.GetUsersAsync();
				var usersJson = JsonSerializer.Serialize(usersFromService);
				await _redisCacheService.SetCacheAsync("users", usersJson);

				return Ok(usersFromService);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error from Get Users {ex.Message}");
			}
		}

		[HttpPost("/Register")]
		public async Task<IActionResult> Register([FromBody] Customers user)
		{
			try
			{
				var newUser = await _customerService.Register(user);
				return Ok(newUser);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error from Post {ex.Message}");
			}
		}
	}
}
