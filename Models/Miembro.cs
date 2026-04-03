using System.ComponentModel.DataAnnotations;

namespace RegistroMiembros.Models
{
    public class Miembro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Telefono { get; set; }
    }
}
