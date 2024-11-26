﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities;

public class Stock
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(10)] 
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public DateTime? MinPriceDate { get; set; }
    public DateTime? MaxPriceDate { get; set; }

    public ICollection<Price> Prices { get; set; } = new List<Price>();
}