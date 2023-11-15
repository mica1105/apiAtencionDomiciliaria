using System.ComponentModel.DataAnnotations;

namespace ApiAtencionDomiciliaria;
public class LoginView
	{
		[DataType(DataType.EmailAddress)]
		public string Usuario { get; set; } = "";
		[DataType(DataType.Password)]
		public string Clave { get; set; } = "";
	}