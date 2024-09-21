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
public class VisitasController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration config;

    public VisitasController(DataContext context, IConfiguration config){
        _context = context;
        this.config = config;
    }
    [HttpGet("fechaAtencion/{fechaAtencion}")]
    public async Task<ActionResult<List<Visita>>> Get(DateOnly fechaAtencion)
    {
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.Visita.Include(x=> x.Paciente)
            .Where(x=> x.Enfermero.Email == usuario && x.FechaAtencion == fechaAtencion)
            .ToListAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("cantidad/{fechaAtencion}")]
    public async Task<ActionResult<int>> GetCantidad(DateOnly fechaAtencion){
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.Visita
            .Where(x=> x.Enfermero.Email == usuario && x.Estado == false && x.FechaAtencion == fechaAtencion)
            .CountAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Visita>> Get(int id)
    {
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.Visita.AsNoTracking().Where(x=> x.Enfermero.Email == usuario && x.Id == id).FirstOrDefaultAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post(Visita visita){
        try{
            if(ModelState.IsValid){
                var usuario = User.Identity.Name;
                visita.EnfermeroId= _context.Enfermero.AsNoTracking().Where(x => x.Email == usuario).First().Id;
                visita.Estado= false;
                _context.Visita.Add(visita);
                await _context.SaveChangesAsync();
                return Ok(visita);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id,Visita visita){
        try{
            if(ModelState.IsValid && _context.Visita.AsNoTracking().FirstOrDefault(X=> X.Id == id) != null){
                visita.EnfermeroId= _context.Enfermero.AsNoTracking()
                .Where(x => x.Email == User.Identity.Name)
                .First().Id;
                if(visita.Estado){
                    return BadRequest("La visita ya fue atendida no es posible modificarla");
                }
                _context.Visita.Update(visita);
                await _context.SaveChangesAsync();
                return Ok(visita);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id){
        try{
            if(_context.Visita.AsNoTracking().FirstOrDefault(X => X.Enfermero.Email == User.Identity.Name) != null){
                var visita = _context.Visita.AsNoTracking()
                .Where(x => x.Enfermero.Email == User.Identity.Name && x.Id == id)
                .First();
                if(visita.Estado){
                    return BadRequest("La visita ya fue atendida no es posible eliminarla");
                }
                _context.Visita.Remove(visita);
                await _context.SaveChangesAsync();
                return Ok("Visita "+ id +" eliminada");    
            }
            return NotFound();
        }
        catch (Exception ex){
            return BadRequest(ex.Message);
        }
    }
}