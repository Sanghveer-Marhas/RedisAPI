using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RedisTestAPI.Models;
using RedisTestAPI.Services;

namespace RedisTestAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		private readonly CustomerService _customerService;

		public CustomerController(CustomerService customerService)
		{
			_customerService = customerService;
		}

		[HttpGet("/UserData")]
		public async Task<ActionResult<Customers>> GetUsersAsync()
		{
			try
			{
				var users = await _customerService.GetUsersAsync();
				return Ok(users);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error from Get Users {ex.Message}");
			}
		}
	}
}
