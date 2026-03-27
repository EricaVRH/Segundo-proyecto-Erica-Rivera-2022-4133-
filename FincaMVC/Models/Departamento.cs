using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FincaMVC.Models
{
    public class Departamento
    {
        [Key]
        public int IdDepartamento{ get; set; }
        public string Nombre { get; set; }
        public int IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public virtual EstadoEmpleado? Estado { get; set; }
    }
}
