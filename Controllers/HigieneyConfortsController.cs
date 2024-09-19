using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAtencionDomiciliaria;
[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class HigieneyConfortsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration config;
    public HigieneyConfortsController(DataContext context, IConfiguration config){
        _context = context;
        this.config = config;
    }

    [HttpGet]
    public async Task<ActionResult<List<HigieneyConfort>>> Get(){
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.HigieneyConfort.AsNoTracking()
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
    public async Task<ActionResult<HigieneyConfort>> Get(int id){
        try
        {
            var usuario = User.Identity.Name;
            var res = await _context.HigieneyConfort.AsNoTracking()
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
    public async Task<ActionResult> Post(HigieneyConfort higieneyConfort){
        try
        {
            if(ModelState.IsValid){
                var usuario = User.Identity.Name;
                var visita = _context.Visita.AsNoTracking().Where(x=> x.Enfermero.Email == usuario && x.Id == higieneyConfort.VisitaId).First();
                higieneyConfort.VisitaId = visita.Id;
                _context.HigieneyConfort.Add(higieneyConfort);
                await _context.SaveChangesAsync();
                return Ok(higieneyConfort);
            }
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, HigieneyConfort higieneyConfort){
        try 
        {
            if(ModelState.IsValid && _context.Visita.AsNoTracking().FirstOrDefault(X => X.Enfermero.Email == User.Identity.Name) != null){
                var usuario = User.Identity.Name;
                var visita = _context.Visita.AsNoTracking().Where(x=> x.Enfermero.Email == usuario && x.Id == higieneyConfort.VisitaId).First();
                higieneyConfort.VisitaId = visita.Id;
                _context.HigieneyConfort.Update(higieneyConfort);
                await _context.SaveChangesAsync();
                return Ok(higieneyConfort);
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
            var res = await _context.HigieneyConfort.AsNoTracking()
            .Where(x=> x.Visita.Enfermero.Email == usuario && x.Id == id)
            .FirstOrDefaultAsync();
            if(res == null) return NotFound();
            _context.HigieneyConfort.Remove(res);
            await _context.SaveChangesAsync();
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}