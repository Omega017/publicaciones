using System.ComponentModel.DataAnnotations;

namespace Publicaciones.Models
{
    public class Persona
    {
        /// <summary>
        /// The National Unique Role 
        /// </summary>
        [Key]
        public string Rut { get; set; }

        /// <summary>
        /// The Email of the person
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The name of the person
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// The second name of the person
        /// </summary>
        public string Apellido { get; set; }
    
    }
}