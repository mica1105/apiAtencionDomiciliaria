using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAtencionDomiciliaria;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CsvsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration config;
    public CsvsController(DataContext context, IConfiguration config){
        _context = context;
        this.config = config;
    }

    [HttpGet]
    public async Task<ActionResult<List<Csv>>> Get(){
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.Csv.AsNoTracking()
            .Where(x=> x.Visita.Enfermero.Email == usuario)
            .ToListAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Csv>> Get(int id){
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.Csv.AsNoTracking()
            .Where(x=> x.Visita.Enfermero.Email == usuario && x.VisitaId == id)
            .FirstOrDefaultAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post(Csv csv){
        try
        {
            if(ModelState.IsValid){
                var usuario = User.Identity.Name;
                var visita = _context.Visita.AsNoTracking().Where(x=> x.Enfermero.Email == usuario && x.Id == csv.VisitaId).First();
                if(visita == null){
                    return NotFound("Visita no encontrada");
                } 
                visita.Estado = true;
                _context.Visita.Update(visita);
                csv.VisitaId = visita.Id;
                _context.Csv.Add(csv);
                await _context.SaveChangesAsync();
                return Ok(csv);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Csv csv){
        try
        {
            if(ModelState.IsValid && _context.Csv.AsNoTracking().FirstOrDefault(X => X.Id == id && X.Visita.Enfermero.Email == User.Identity.Name) != null){
                _context.Csv.Update(csv);
                await _context.SaveChangesAsync();
                return Ok(csv);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {   
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id){
        try
        {
            var usuario = User.Identity.Name;
            var csv = _context.Csv.AsNoTracking()
            .Where(x=> x.Visita.Enfermero.Email == usuario && x.Id == id)
            .First();
            if(csv != null){
                _context.Csv.Remove(csv);
                await _context.SaveChangesAsync();
                return Ok(csv); 
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}