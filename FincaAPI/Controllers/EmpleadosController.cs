using FincaAPI.Data;
using FincaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FincaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly FincaDbContext _context;

        public EmpleadosController(FincaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetEmpleados(
         string? nombre,
         string? apellido,
         DateTime? fechaContratacion,
         int page = 1,
         int pageSize = 5)
        {
            var query = _context.Empleados
                .Include(e => e.Departamento)
                .Include(e => e.Puesto)
                .Include(e => e.EstadoEmpleado)
                .Where(e => e.IdEstado != 3)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(e => e.Nombre.Contains(nombre));

            if (!string.IsNullOrEmpty(apellido))
                query = query.Where(e => e.Apellido.Contains(apellido));

            if (fechaContratacion.HasValue)
                query = query.Where(e => e.FechaContratacion.Date == fechaContratacion.Value.Date);

            var total = await query.CountAsync();

            var empleados = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                total,
                page,
                pageSize,
                data = empleados
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Departamento)
                .Include(e => e.Puesto)
                .Include(e => e.EstadoEmpleado)
                .FirstOrDefaultAsync(e => e.IdEmpleado == id);

            if (empleado == null)
                return NotFound();

            return empleado;
        }

        [HttpPost]
        public async Task<ActionResult<Empleado>> PostEmpleado(Empleado empleado)
        {
            ModelState.Remove(nameof(Empleado.Departamento));
            ModelState.Remove(nameof(Empleado.Puesto));
            ModelState.Remove(nameof(Empleado.EstadoEmpleado));
            ModelState.Clear();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmpleado), new { id = empleado.IdEmpleado }, empleado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpleado(int id, [FromBody] Empleado empleado)
        {
            if (id != empleado.IdEmpleado)
                return BadRequest();

            var empleadoBD = await _context.Empleados.FindAsync(id);
            if (empleadoBD == null)
                return NotFound();

            // Actualizar campos
            empleadoBD.Nombre = empleado.Nombre;
            empleadoBD.Apellido = empleado.Apellido;
            empleadoBD.IdDepartamento = empleado.IdDepartamento;
            empleadoBD.IdPuesto = empleado.IdPuesto;
            empleadoBD.Salario = empleado.Salario;
            empleadoBD.FechaNacimiento = empleado.FechaNacimiento;
            empleadoBD.FechaContratacion = empleado.FechaContratacion;
            empleadoBD.Direccion = empleado.Direccion;
            empleadoBD.EstadoEmpleado= empleado.EstadoEmpleado;
            empleadoBD.Telefono = empleado.Telefono;
            empleadoBD.Correo = empleado.Correo;

            // Actualiza foto solo si viene
            if (!string.IsNullOrEmpty(empleado.FotoRuta))
                empleadoBD.FotoRuta = empleado.FotoRuta;

            _context.Update(empleadoBD);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
                return NotFound();

            empleado.IdEstado = 2;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("activar/{id}")]
        public async Task<IActionResult> ActivarEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
                return NotFound();

            empleado.IdEstado = 1; // Activar
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("despedir/{id}")]
        public async Task<IActionResult> DespedirEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
                return NotFound();

            empleado.IdEstado = 3;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

