using FincaAPI.Data;
using FincaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FincaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly FincaDbContext _context;

        public EstadosController(FincaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoEmpleado>>> GetEstados()
        {
            return await _context.EstadosEmpleado.ToListAsync();
        }
    }
}
