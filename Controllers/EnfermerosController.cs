using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiAtencionDomiciliaria;
[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EnfermerosController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration config;
    private readonly IWebHostEnvironment environment;

    public EnfermerosController(DataContext context, IConfiguration config, IWebHostEnvironment environment){
        _context = context;
        this.config = config;
        this.environment = environment;
    }

    [HttpPost("Crear")]
    [AllowAnonymous]
    public async Task<ActionResult> CrearEnfermero([FromForm] Enfermero enfermero){
        try
        {
            if(ModelState.IsValid){
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: enfermero.Password,			
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8
					));
                enfermero.Password=hashed;
                await _context.Enfermero.AddAsync(enfermero);
                _context.SaveChanges();
                if(enfermero.AvatarFile!=null && enfermero.Id !=0){
                    string wwwRootPath = environment.WebRootPath;
                    string path = Path.Combine(wwwRootPath, "Uploads");
                    if(!Directory.Exists(path)){
                        Directory.CreateDirectory(path);
                    }
                    string fileName ="enfermero"+enfermero.Id+ Path.GetExtension(enfermero.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    using(var fileStream = new FileStream(pathCompleto, FileMode.Create)){
                        await enfermero.AvatarFile.CopyToAsync(fileStream);
                    }
                    enfermero.Avatar=Path.Combine("/Uploads", fileName);
                    _context.Enfermero.Update(enfermero);
                    await _context.SaveChangesAsync();
                    
                }
                return CreatedAtAction(nameof(CrearEnfermero), new {id=enfermero.Id},enfermero);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("Login")]
    [AllowAnonymous]
	public async Task<IActionResult> Login([FromForm] LoginView loginView)
	{
		try
		{
			if (ModelState.IsValid){
				
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: loginView.Clave,
						salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8
					));
				Enfermero p = await _context.Enfermero.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
				if (p == null || p.Password != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}	
																					
                var key = new SymmetricSecurityKey(
                    System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));

                var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, p.Email),
                    new Claim("FullName", p.Nombre + " " + p.Apellido),
                };
                
                var token = new JwtSecurityToken(
                    issuer: config["TokenAuthentication:Issuer"],
                    audience: config["TokenAuthentication:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credenciales
                );

				return Ok(new JwtSecurityTokenHandler().WriteToken(token));
			}
			return BadRequest();
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

    [HttpGet]
    public async Task<ActionResult<List<Enfermero>>> Get(){
        try{
            var usuario= User.Identity.Name;
            var res= await _context.Enfermero.SingleOrDefaultAsync(x=> x.Email == usuario);
            return Ok(res);

        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Put(Enfermero enfermero){
        try
        {
            if(ModelState.IsValid){
                var usuario= _context.Enfermero.AsNoTracking().Where(x => x.Email == User.Identity.Name).First();
                enfermero.Id= usuario.Id;
                enfermero.Email= usuario.Email;
                enfermero.Password= usuario.Password;
                _context.Enfermero.Update(enfermero);
                await _context.SaveChangesAsync();
                return Ok(enfermero);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("CambiarClave")]
    public async Task<ActionResult> CambiarClave([FromForm] string claveActual, [FromForm] string nuevaClave){
        try
        {
                var usuario= _context.Enfermero.SingleOrDefault(x=> x.Email == User.Identity.Name);
                if(Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password:claveActual,
                    salt:System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf:KeyDerivationPrf.HMACSHA1,
                    iterationCount:1000,
                    numBytesRequested:256/8
                )) != usuario.Password){
                    return BadRequest("La clave actual es incorrecta");
                }
                string hashed= Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password:nuevaClave,
                    salt:System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf:KeyDerivationPrf.HMACSHA1,
                    iterationCount:1000,
                    numBytesRequested:256/8
                ));
                usuario.Password= hashed;
                _context.Enfermero.Update(usuario);
                await _context.SaveChangesAsync();
                var key = new SymmetricSecurityKey(
                    System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));

                var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                };
                
                var token = new JwtSecurityToken(
                    issuer: config["TokenAuthentication:Issuer"],
                    audience: config["TokenAuthentication:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credenciales
                );
                
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        catch (Exception ex){
            return StatusCode(500, ex.Message);
        }
    }
}