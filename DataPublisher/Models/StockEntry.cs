using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataPublisher.Models;

public class StockEntry
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    //public string StockName { get; set; }
    public string MarketName { get; set; } = "NASDAQ";
    //public DateTime FromDate { get; set; }
    //public DateTime ToDate { get; set; }
}