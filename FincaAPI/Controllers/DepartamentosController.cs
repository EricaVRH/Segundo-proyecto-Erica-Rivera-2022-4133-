using FincaAPI.Data;
using FincaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FincaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private readonly FincaDbContext _context;
        public DepartamentosController(FincaDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetDepartamentos()
            => await _context.Departamentos.Where(d => d.IdEstado != 2).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Departamento>> GetDepartamento(int id)
        {
            var dep = await _context.Departamentos.FindAsync(id);
            if (dep == null) return NotFound();
            return dep;
        }

        [HttpPost]
        public async Task<ActionResult<Departamento>> PostDepartamento(Departamento dep)
        {
            _context.Departamentos.Add(dep);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDepartamento), new { id = dep.IdDepartamento }, dep);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartamento(int id, Departamento dep)
        {
            if (id != dep.IdDepartamento) return BadRequest();

            var depBD = await _context.Departamentos.FindAsync(id);
            if (depBD == null) return NotFound();

            depBD.Nombre = dep.Nombre;
            

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            var dep = await _context.Departamentos.FindAsync(id);
            if (dep == null) return NotFound();

            dep.IdEstado = 2; // Inactivo
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("activar/{id}")]
        public async Task<IActionResult> ActivarDepartamento(int id)
        {
            var dep = await _context.Departamentos.FindAsync(id);
            if (dep == null) return NotFound();

            dep.IdEstado = 1;
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
