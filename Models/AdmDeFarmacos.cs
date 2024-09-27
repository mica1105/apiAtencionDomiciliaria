using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAtencionDomiciliaria;

public class AdmDeFarmacos
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Via { get; set; } = "";
    [Required]
    public string? Medicacion { get; set; } = "";
    public string? Ra { get; set; } = "";
    [Required]
    public float Dosis { get; set; }
    public string? Observaciones { get; set; } = "";
    [Required]
    public int VisitaId { get; set; }
    [ForeignKey("VisitaId")]
    public Visita? Visita { get; set; }
}