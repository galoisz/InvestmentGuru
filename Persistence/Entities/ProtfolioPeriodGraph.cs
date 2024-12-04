using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Entities;

public class ProtfolioPeriodGraph
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ProtfolioPeriodId { get; set; }  // Foreign key to ProtfolioPeriod

    [Required]
    public string Graphdata { get; set; } = string.Empty;

    // Navigation property
    [ForeignKey(nameof(ProtfolioPeriodId))]
    public ProtfolioPeriod ProtfolioPeriod { get; set; } = null!;
}