using DataConsumer.Services.StockPrices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConsumer.Services.StockPrices;

public interface IStockPriceService
{
    Task<List<StockPrice>> GetStockPrices(string symbol, DateTime startDate, DateTime endDate);

}
