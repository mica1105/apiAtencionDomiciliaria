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
public class HCController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration config;

    public HCController(DataContext context, IConfiguration config){
        _context = context;
        this.config = config;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id){
        try
        {
            var res = await _context.HC.Include(x => x.Paciente).SingleOrDefaultAsync(x => x.PacienteId == id);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}