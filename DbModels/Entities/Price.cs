using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModels.Entities;

public class Price
{
    [Key] // Marks as Primary Key
    public Guid Id { get; set; }

    [Required]
    public Guid StockId { get; set; } // Foreign Key

    [Required]
    public DateTime PriceDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")] // Ensures proper precision in the database
    public decimal PriceValue { get; set; }

    // Navigation property
    [ForeignKey(nameof(StockId))]
    public Stock Stock { get; set; } = null!;
}