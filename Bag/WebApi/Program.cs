using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.Repositories;
using WebApi.Helpers.Finance;
using WebApi.Services;
using WebApi.Services.Finance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("StocksConnection")));

builder.Services.Configure<FinanceApiOptions>(builder.Configuration.GetSection("FinanceApi"));

// Register the finance service
builder.Services.AddScoped<IFinanceService, FinanceService>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddScoped<IStockDataService, StockDataService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
