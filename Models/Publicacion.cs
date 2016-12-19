using System.Collections.Generic;
using System;
namespace Publicaciones.Models
{
    public class Publicacion
    {
        public string PublicacionId { get; set; }

        public DateTime FechaRevista { get; set; }
        
        public string RevistaId { get; set; }
        public virtual Revista Revista { get; set; }

        public virtual List < Autor > Autor { get; set; }

        public DateTime FechaWeb { get; set; }

        public int PagInicio { get; set; }

        public virtual List < Paper > Papers { get; set; }

    }
}