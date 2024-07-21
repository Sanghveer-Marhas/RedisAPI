using Microsoft.EntityFrameworkCore;
using RedisTestAPI.Models;

namespace RedisTestAPI.Services
{
	public class CustomerService
	{

		private readonly AppDBContext _context;

		public CustomerService(AppDBContext context)
		{
			_context = context;
		}

		public async Task<List<Customers>> GetUsersAsync()
		{

				return await _context.customers.ToListAsync();
			
		}
	}
}
