using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FincaAPI.Models
{
    public class Departamento
    {
        [Key]
        public int IdDepartamento{ get; set; }

        [Required(ErrorMessage = "El nombre del departamento es obligatorio")]
        public string Nombre { get; set; }
        public int IdEstado { get; set; }
                     
        [ForeignKey("IdEstado")]
        public virtual EstadoEmpleado? Estado { get; set; }
    }
}
