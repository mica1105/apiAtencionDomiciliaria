using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAtencionDomiciliaria;

public class Paciente
{
		
	[Key]
	[Display(Name = "Código")]
	public int Id { get; set; }	
	[Required]
	public string Nombre { get; set; }  = "";	
	[Required]
	public string Apellido { get; set; } = "";	
	[Required]
	public int Dni { get; set; }
    [DataType(DataType.Date)]
	public DateOnly FechaNacimiento { get; set; } 
	[Required]
	public string Domicilio { get; set; } = "";
    public long Telefono { get; set; }
	[DataType(DataType.Date)]
	public DateOnly FechaRegistro { get; set; } 
	[DataType(DataType.Date)]
	public DateOnly FechaModificacion { get; set; }
	public int EstadoId { get; set; }
	[ForeignKey("EstadoId")]
	public Estado? Estado {get; set;}

    public int CalcularEdad(){
		return DateOnly.FromDateTime(DateTime.Now).Year - FechaNacimiento.Year;
    }
}