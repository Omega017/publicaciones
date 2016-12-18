using System;

namespace Publicaciones.Models
{
    public class Paper
    {
        public string PaperId { get; set; }

        public string Titulo { get; set; }

        public string Abstract { get; set; }

        public string LineaInvestigativa { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaTermino { get; set; }

        public string PublicacionId { get; set; }
        public virtual Publicacion Publicacion { get; set; }
    }
}