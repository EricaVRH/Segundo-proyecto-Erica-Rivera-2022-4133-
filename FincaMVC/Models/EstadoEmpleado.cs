using System.ComponentModel.DataAnnotations;

namespace FincaMVC.Models
{
    public class EstadoEmpleado
    {
        [Key]
        public int IdEstado { get; set; }

        public string Nombre { get; set; }
    }
}
