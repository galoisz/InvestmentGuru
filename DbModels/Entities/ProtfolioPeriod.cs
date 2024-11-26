using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;


public class ProtfolioPeriod
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ProtfolioId { get; set; }  // Foreign key to Protfolio

    [Column(TypeName = "date")]
    public DateTime FromDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime ToDate { get; set; }

    // Navigation property
    public Protfolio Protfolio { get; set; }
}