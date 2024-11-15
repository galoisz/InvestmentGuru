using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.Entities;

public class Stock
{
    [Key] // Marks as Primary Key
    public Guid Id { get; set; }

    [Required] // Makes the column NOT NULL
    [MaxLength(10)] // Sets a maximum length constraint
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public DateTime? MinPriceDate { get; set; }
    public DateTime? MaxPriceDate { get; set; }

    // Navigation property
    public ICollection<Price> Prices { get; set; } = new List<Price>();
}