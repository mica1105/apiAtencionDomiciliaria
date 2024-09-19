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
public class PacientesController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration config;

    public PacientesController(DataContext context, IConfiguration config){
        _context = context;
        this.config = config;
    }

    [HttpGet]
    public async Task<ActionResult<List<Paciente>>> Get(){
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.Paciente.AsNoTracking().ToListAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Atendidos")]
    public async Task<ActionResult<List<Paciente>>> GetAtendidos(){
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.Visita.AsNoTracking()
            .Where(x=> x.Enfermero.Email == usuario)
            .Select(x=> x.Paciente)
            .Distinct()
            .ToListAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Buscar/{nombre}")]
    public async Task<ActionResult<List<Paciente>>> GetBuscar(string nombre){
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.Paciente.AsNoTracking().Where(x=> x.Nombre.Contains(nombre) || x.Apellido.Contains(nombre)).ToListAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromForm] Paciente paciente){
        try
        {
            if(ModelState.IsValid){
                await _context.Paciente.AddAsync(paciente);
                await _context.SaveChangesAsync();
                return Ok(paciente);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
}