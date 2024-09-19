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
                admDeFarmacos.VisitaId = _context.Visita.AsNoTracking()
                .Where(x=> x.Enfermero.Email == usuario && x.Id == admDeFarmacos.VisitaId)
                .First().Id;
                _context.AdmDeFarmacos.Add(admDeFarmacos);
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

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id,AdmDeFarmacos admDeFarmacos){
        try
        {
            if(ModelState.IsValid && _context.AdmDeFarmacos.AsNoTracking().FirstOrDefault(X => X.Id == id && X.Visita.Enfermero.Email == User.Identity.Name) != null){
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