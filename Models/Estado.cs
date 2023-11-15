using System.ComponentModel.DataAnnotations;

namespace ApiAtencionDomiciliaria;

public class Estado
{
    [Key]
	public int Id { get; set; }
    
    public string? Descripcion { get; set; }
}
