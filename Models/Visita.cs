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
	[DataType(DataType.Date)]
	public DateOnly? FechaAtencion { get; set; } 
	[Required]
	[DataType(DataType.Time)]
	public TimeOnly InicioAtencion { get; set; }
    [Required]
	[DataType(DataType.Time)]
	public TimeOnly FinAtencion { get; set; }
	public bool Estado {get; set;}
    [DataType(DataType.Date)]
	public DateOnly FechaCreacion { get; set; } 
	[DataType(DataType.Date)]
	public DateOnly FechaModificacion { get; set; }
	[Required]
	public string Prestaciones { get; set; } = "";
    public int PacienteId { get; set; }
	[ForeignKey("PacienteId")]
    public Paciente? Paciente { get; set; }
    public int EnfermeroId { get; set; }
	[ForeignKey("EnfermeroId")]
    public Enfermero? Enfermero { get; set; }

    public static implicit operator Visita(Task<Visita?> v)
    {
        throw new NotImplementedException();
    }
}