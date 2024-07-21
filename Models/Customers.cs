using System.ComponentModel.DataAnnotations;

namespace RedisTestAPI.Models
{
	public class Customers
	{

		[Key]
		public int id { get; set; }
		public string? name { get; set; }
		public string? email {  get; set; }
	}
}
