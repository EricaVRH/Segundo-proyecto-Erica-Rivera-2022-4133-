using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FincaMVC.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public int IdDepartamento { get; set; }
        public int IdPuesto { get; set; }
        public int IdEstado { get; set; }

        public decimal Salario { get; set; }

        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaContratacion { get; set; }

        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

        public string? FotoRuta { get; set; }

        [NotMapped]
        public IFormFile Foto { get; set; }

        // Relaciones
        [ForeignKey("IdDepartamento")]
        public Departamento? Departamento { get; set; }

        [ForeignKey("IdPuesto")]
        public Puesto? Puesto { get; set; }

        [ForeignKey("IdEstado")]
        public EstadoEmpleado? EstadoEmpleado { get; set; }


    }
}
