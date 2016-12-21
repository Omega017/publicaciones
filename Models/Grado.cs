using System;
namespace Publicaciones.Models
{
    public class Grado
    {
        public string GradoId { get; set; }

        public string Nombre { get; set; }

        public DateTime Fecha { get; set; }

        public string PersonaId { get; set ;}
        public Persona Persona { get; set; }
    }
}