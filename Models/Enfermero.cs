using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAtencionDomiciliaria;

public class Enfermero
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

	public string? Telefono { get; set; }
	[Required]
	public string Domicilio { get; set; } = "";

	[Required, EmailAddress]
	public string Email { get; set; } = "";
	
	[Required, DataType(DataType.Password)]
	public string Password { get; set; } = "";
	public string Avatar { get; set; }= "";
	[NotMapped]
	public IFormFile? AvatarFile { get; set; }
	public int EstadoId { get; set; }
	[ForeignKey("EstadoId")]
	public Estado? Estado {get; set;}
}
        
