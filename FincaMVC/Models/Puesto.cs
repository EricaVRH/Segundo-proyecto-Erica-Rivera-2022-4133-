using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FincaMVC.Models
{
    public class Puesto
    {
        [Key]
        public int IdPuesto { get; set; }

        public string Nombre { get; set; }

        public int IdDepartamento { get; set; }
        public int IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public virtual EstadoEmpleado? Estado { get; set; }

        [ForeignKey("IdDepartamento")]
        public Departamento? Departamento { get; set; }
    }
}
