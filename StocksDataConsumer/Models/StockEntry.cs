﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConsumer.Models;

public class StockEntry
{
    public string Symbol { get; set; }
    public string MarketName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}