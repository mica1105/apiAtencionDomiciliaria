using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAtencionDomiciliaria;

public class Csv
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Ta { get; set; } = "";
    [Required]
    public int Fc { get; set; }
    [Required]
    public int So2 { get; set; }
    [Required]
    public float Temp { get; set; }
    [Required]
    public int Hgt { get; set; }
    [Required]
    public string? Observaciones { get; set; }
    [Required]
    public int VisitaId { get; set; }
    [ForeignKey("VisitaId")]
    public Visita? Visita { get; set; }
}