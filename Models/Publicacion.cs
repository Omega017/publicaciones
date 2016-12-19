using System.Collections.Generic;
using System;
namespace Publicaciones.Models
{
    public class Publicacion
    {
        /// <summary>
        /// PublicacionId is the identifier of this class
        /// </summary>
        public string PublicacionId { get; set; }

        /// <summary>
        /// The date that the publication appeared in the magazine
        /// </summary>
        public DateTime FechaRevista { get; set; }

        /// <summary>
        /// Is an identifier of the magazine
        /// </summary>
        public string RevistaId { get; set; }

        /// <summary>
        /// Revista is the navigation attribute from Revista
        /// </summary>
        public virtual Revista Revista { get; set; }

        /// <summary>
        /// The list of the present authors of the publication 
        /// </summary>
        public virtual List < Autor > Autor { get; set; }

        /// <summary>
        /// The date when the publication appeared in the DataBase of the WebSite
        /// </summary>
        public DateTime FechaWeb { get; set; }

        /// <summary>
        /// The page of the magazine where the publication start
        /// </summary>
        public int PagInicio { get; set; }

        /// <summary>
        /// List of papers 
        /// </summary>
        public virtual List < Paper > Papers { get; set; }

    }
}