using System;

namespace Publicaciones.Models
{
    public class Paper
    {
        /// <summary>
        /// PaperId is the identifier of this class
        /// </summary>
        public string PaperId { get; set; }

        /// <summary>
        /// Titulo is the name of the Paper
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// This is the Abstract of the paper that introduce to this
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// LineaInvestigativa is a categorization of the paper
        /// </summary>
        public string LineaInvestigativa { get; set; }

        /// <summary>
        /// The AreaDesarrollo is a categorization of the paper
        /// </summary>
        public string AreaDesarrollo { get; set; }

        /// <summary>
        /// FechaInicio is the Date that this paper was start to write
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// FechaTermino is the Date that this paper was finish
        /// </summary>
        public DateTime FechaTermino { get; set; }

        /// <summary>
        /// It is an identifier that relate the Paper with the Publicacion
        /// </summary>
        public string PublicacionId { get; set; }

        /// <summary>
        /// Publicacion is the navigation attribute from Publicacion
        /// </summary>
        public virtual Publicacion Publicacion { get; set; }
    }
}