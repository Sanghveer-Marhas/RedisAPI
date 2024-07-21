using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RedisAPI2.Services;
using RedisTestAPI.Models;
using RedisTestAPI.Services;
using Serilog;
using Serilog.Formatting.Compact;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var redisConfiguration = builder.Configuration.GetSection("ConnectionStrings:Redis").Value;
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));

builder.Services.AddSingleton<RedisService>();

Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Information()
			.WriteTo.Console(new CompactJsonFormatter())
			.Enrich.FromLogContext()
			.CreateLogger();

builder.Services.AddDbContext<AppDBContext>(
	o => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB")));

builder.Services.AddScoped<CustomerService>();

/*
ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
StackExchange.Redis.IDatabase db = redis.GetDatabase();
db.StringSet("mykey", "Hello, Redis!");
string value = db.StringGet("mykey");
Console.WriteLine($"The value of 'mykey' is: {value}");
*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
