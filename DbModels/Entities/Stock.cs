using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.Entities;

public class Stock
{
    public Guid Id { get; set; }
    public string Symbol { get; set; }
    public string Prices { get; set; } // JSON representation of List<PriceEntry>
}