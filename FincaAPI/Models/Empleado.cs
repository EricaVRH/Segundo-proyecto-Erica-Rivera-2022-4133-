using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; 

namespace FincaAPI.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un departamento")]
        public int IdDepartamento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un puesto")]
        public int IdPuesto { get; set; }

        public int IdEstado { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Range(0, 1000000, ErrorMessage = "El salario debe ser mayor o igual a 0")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La fecha de contratación es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "El teléfono no es válido")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Correo { get; set; }

        public string? FotoRuta { get; set; }


        [JsonIgnore]
        [ForeignKey("IdDepartamento")]
        public Departamento? Departamento { get; set; }

        [JsonIgnore]
        [ForeignKey("IdPuesto")]
        public Puesto? Puesto { get; set; }

        [JsonIgnore]
        [ForeignKey("IdEstado")]
        public EstadoEmpleado? EstadoEmpleado { get; set; }
    }
}