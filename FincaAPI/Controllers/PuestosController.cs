using FincaAPI.Data;
using FincaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FincaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuestosController : ControllerBase
    {
        private readonly FincaDbContext _context;
        public PuestosController(FincaDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Puesto>>> GetPuestos()
            => await _context.Puestos.Include(p => p.Departamento)
                .Where(p => p.IdEstado != 2).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Puesto>> GetPuesto(int id)
        {
            var puesto = await _context.Puestos.Include(p => p.Departamento)
                .FirstOrDefaultAsync(p => p.IdPuesto == id);
            if (puesto == null) return NotFound();
            return puesto;
        }

        [HttpGet("por-departamento/{idDepartamento}")]
        public async Task<ActionResult<IEnumerable<Puesto>>> GetPuestosPorDepartamento(int idDepartamento)
            => await _context.Puestos
                .Where(p => p.IdDepartamento == idDepartamento && p.IdEstado != 2)
                .ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Puesto>> PostPuesto(Puesto puesto)
        {
            _context.Puestos.Add(puesto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPuesto), new { id = puesto.IdPuesto }, puesto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPuesto(int id, Puesto puesto)
        {
            if (id != puesto.IdPuesto) return BadRequest();
            var puestoBD = await _context.Puestos.FindAsync(id);
            if (puestoBD == null) return NotFound();

            puestoBD.Nombre = puesto.Nombre;
            puestoBD.IdDepartamento = puesto.IdDepartamento;
            

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePuesto(int id)
        {
            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null) return NotFound();

            puesto.IdEstado = 2; // Inactivo
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("activar/{id}")]
        public async Task<IActionResult> ActivarPuesto(int id)
        {
            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null) return NotFound();

            puesto.IdEstado = 1; // Activo
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
