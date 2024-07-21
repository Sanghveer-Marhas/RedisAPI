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

		public async Task<Customers> Register(Customers request)
		{

			try
			{
				var user = new Customers();
				string emailHash
				= BCrypt.Net.BCrypt.HashPassword(request.email);

				user.name = request.name;
				user.email = emailHash;

				await _context.customers.AddAsync(user);
				await _context.SaveChangesAsync();

				return user;
			}
			catch (Exception ex)
			{
				throw new Exception("Error from Post", ex);
			}

		}
	}
}
