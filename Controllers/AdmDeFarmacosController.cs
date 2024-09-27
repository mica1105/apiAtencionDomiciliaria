using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAtencionDomiciliaria;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdmDeFarmacosController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration config;
    public AdmDeFarmacosController(DataContext context, IConfiguration config){
        _context = context;
        this.config = config;
    }

    [HttpGet]
    public async Task<ActionResult<List<AdmDeFarmacos>>> Get(){
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.AdmDeFarmacos.AsNoTracking()
            .Include(x=> x.Visita)
            .ThenInclude(x=> x.Paciente)
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
    public async Task<ActionResult<AdmDeFarmacos>> Get(int id){
        try
        {   
            var usuario = User.Identity.Name;
            var res = await _context.AdmDeFarmacos.AsNoTracking()
            .Include(x=> x.Visita)
            .ThenInclude(x=> x.Paciente)
            .Where(x=> x.Visita.Enfermero.Email == usuario && x.Id == id)
            .FirstOrDefaultAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post(AdmDeFarmacos admDeFarmacos){
        try 
        {
            if(ModelState.IsValid){
                var usuario = User.Identity.Name;
                var visita = _context.Visita.FirstOrDefault(x => x.Enfermero.Email == usuario && x.Id == admDeFarmacos.VisitaId);

                if (visita == null)
                {
                    return NotFound("Visita no encontrada.");
                }

                admDeFarmacos.VisitaId = visita.Id;
                _context.AdmDeFarmacos.Add(admDeFarmacos);

                // Actualizar el estado de la visita
                visita.Estado = true;
                visita.FechaModificacion = DateOnly.FromDateTime(DateTime.Now);
                _context.Visita.Update(visita);
                await _context.SaveChangesAsync();
                return Ok(admDeFarmacos);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {   
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult> Put(AdmDeFarmacos admDeFarmacos){
        try
        {
            if(ModelState.IsValid && _context.AdmDeFarmacos.AsNoTracking().FirstOrDefault(X => X.Id == admDeFarmacos.Id && X.Visita.Enfermero.Email == User.Identity.Name) != null){
                var visita= _context.Visita
                .Include(x=> x.Paciente)
                .FirstOrDefault(x => x.Enfermero.Email == User.Identity.Name && x.Id == admDeFarmacos.VisitaId);
                admDeFarmacos.Visita = visita;
                _context.AdmDeFarmacos.Update(admDeFarmacos);
                await _context.SaveChangesAsync();
                return Ok(admDeFarmacos);
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
            var res = await _context.AdmDeFarmacos
            .Where(x=> x.Id == id)
            .FirstOrDefaultAsync();
            if(res == null) return NotFound();
            _context.AdmDeFarmacos.Remove(res);
            await _context.SaveChangesAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}