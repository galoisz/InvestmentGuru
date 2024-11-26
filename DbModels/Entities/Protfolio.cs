using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

public class Protfolio
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }  // Foreign key to aspnetUsers

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; }

    // Navigation properties
    public ICollection<ProtfolioStock> Stocks { get; set; } = new List<ProtfolioStock>();
    public ICollection<ProtfolioPeriod> Periods { get; set; } = new List<ProtfolioPeriod>();
}

