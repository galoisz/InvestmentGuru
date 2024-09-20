using DataPublisher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPublisher.Services;

public interface IStocksDataService
{
    IEnumerable<StockEntry> GetStockData();
}