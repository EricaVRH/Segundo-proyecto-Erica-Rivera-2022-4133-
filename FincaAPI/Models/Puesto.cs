using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FincaAPI.Models
{
    public class Puesto
    {
        [Key]
        public int IdPuesto { get; set; }


        [Required(ErrorMessage = "El nombre del puesto es obligatorio")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El id del departamento es obligatorio")]
        public int IdDepartamento { get; set; }
        public int IdEstado {  get; set; }

        [ForeignKey("IdDepartamento")]
        public Departamento? Departamento { get; set; }
        [ForeignKey("IdEstado")]
        public virtual EstadoEmpleado? Estado { get; set; }
    }
}
