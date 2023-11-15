using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAtencionDomiciliaria;

public class HC
{
		
	[Key]
	public int Id { get; set; }
    public string Diagnostico { get; set; }="";
    public string Medicacion { get; set; }="";
    public bool Fuma { get; set; }
    public bool Bebe { get; set; }
    public bool Drogas { get; set; }
    public bool Dbt { get; set; }
    public bool Hta { get; set; }
    public string Alergias { get; set; }="";
    public string Traumas { get; set; }="";
    public string Cirugias { get; set; }="";
    public int PacienteId { get; set; }
    [ForeignKey("PacienteId")]
    public Paciente? Paciente { get; set; }
}