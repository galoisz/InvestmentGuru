using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;
public class Price
{
    [Key] // Marks this property as the primary key
    public Guid Id { get; set; }

    [Required] // Makes this column NOT NULL
    public Guid StockId { get; set; } // Foreign Key to Stock

    [Required]
    public string Value { get; set; } = string.Empty; // JSON data or other value

    // Navigation property
    [ForeignKey(nameof(StockId))]
    public Stock Stock { get; set; } = null!;
}
