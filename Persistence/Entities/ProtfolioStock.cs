using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

public class ProtfolioStock
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ProtfolioId { get; set; }  // Foreign key to Protfolio

    [Required]
    public Guid StockId { get; set; }  // Foreign key to Stock

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal Ratio { get; set; }

    // Navigation properties
    [ForeignKey(nameof(ProtfolioId))]
    public Protfolio Protfolio { get; set; }

    [ForeignKey(nameof(StockId))]
    public Stock Stock { get; set; }
}
