namespace Publicaciones.Models
{
    public class Autor
    {
        /// <summary>
        /// The id of the Autor
        /// </summary>
        public string AutorId { get; set; }

        /// <summary>
        /// The type of the Autor that could be one of the enum defined on this class
        /// </summary>
        public TipoAutor TipoAutor { get; set; }

        /// <summary>
        /// The National Unique Role 
        /// </summary>
        public string Rut { get; set; }

        /// <summary>
        /// Who is the Autor
        /// </summary>
        public virtual Persona Persona { get; set; }

        /// <summary>
        /// PublicacionId is the Id that reference one publication
        /// </summary>
        public string PublicacionId { get; set; }

        /// <summary>
        /// Publicacion is the navigation attribute from publication
        /// </summary>
        public virtual Publicacion Publicacion { get; set; }
    }
    /// <summary>
    /// TipoAutor is an enumeration that use this attribute
    /// </summary>
    public enum TipoAutor { Principal, Correspondiente, Normal };
}