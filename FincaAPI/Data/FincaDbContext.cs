using FincaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FincaAPI.Data
{
    public class FincaDbContext : DbContext
    {
        public FincaDbContext(DbContextOptions<FincaDbContext> options): base(options){}

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<EstadoEmpleado> EstadosEmpleado { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Puesto>().ToTable("Puesto");
            modelBuilder.Entity<Departamento>().ToTable("Departamento");
            modelBuilder.Entity<EstadoEmpleado>().ToTable("EstadoEmpleado");
        }
    }
}
