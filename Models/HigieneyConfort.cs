using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiAtencionDomiciliaria;

public class HigieneyConfort
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Tipo { get; set; } = "";

    public string? Materiales { get; set; } = "";
    [Required]
    public bool? Paniales { get; set; } = false;
    [Required]
    public bool? SondaVesical { get; set; } = false;
    [Required]
    public bool? SondaNasogastrica { get; set; } = false;
    public string? Observaciones { get; set; } = "";
    [Required]
    public int VisitaId { get; set; }
    [ForeignKey("VisitaId")]
    public Visita? Visita { get; set; }
}