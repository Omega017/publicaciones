using System.ComponentModel.DataAnnotations;

namespace Publicaciones.Models
{
    public class Persona
    {
        [Key]
        public string Rut { get; set; }

        public string Email { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }
    
    }
}