namespace Publicaciones.Models
{
    public class Autor
    {
        public string AutorId { get; set; }

        public TipoAutor TipoAutor { get; set; }

        public string Rut { get; set; }
        public virtual Persona Persona { get; set; }

        public string PublicacionId { get; set; }
        public virtual Publicacion Publicacion { get; set; }
    }

    public enum TipoAutor { Principal, Correspondiente, Normal };
}