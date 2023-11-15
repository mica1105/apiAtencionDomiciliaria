using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAtencionDomiciliaria;

public class Visita
{
		
	[Key]
	public int Id { get; set; }
	[Required]
	[DataType(DataType.Date)]
	public DateOnly FechaAtencion { get; set; } 
	[Required]
	[DataType(DataType.Time)]
	public TimeOnly HoraInicio { get; set; }
    [Required]
	[DataType(DataType.Time)]
	public TimeOnly HoraFin { get; set; }
	[Required]
	public bool Estado {get; set;}
    [DataType(DataType.Date)]
	public DateOnly FechaRegistro { get; set; } 
	[DataType(DataType.Date)]
	public DateOnly FechaModificacion { get; set; }
    public int PacienteId { get; set; }
	[ForeignKey("PacienteId")]
    public Paciente Paciente { get; set; }
    public int EnfermeroId { get; set; }
	[ForeignKey("EnfermeroId")]
    public Enfermero Enfermero { get; set; }
}