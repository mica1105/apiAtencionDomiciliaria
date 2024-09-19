using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAtencionDomiciliaria;
public class Curacion {
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Tipo { get; set; } = "";
    [Required]
    public string? Ubicacion { get; set; } = "";
    [Required]
    public string? Clase { get; set; } = "";
    [Required]
    public decimal? Tamaño { get; set; } = 0;
    [Required]
    public string? Bordes { get; set; } = "";
    [Required]
    public string? SignosInfeccion { get; set; } = "";
    [Required]
    public string? Dolor { get; set; } = "";
    public string? Observaciones { get; set; } = "";
    [Required]
    public int VisitaId { get; set; }
    [ForeignKey("VisitaId")]
    public Visita? Visita { get; set; }

}