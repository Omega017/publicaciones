namespace Publicaciones.Models
{
    public class Autor
    {
        public string Id { get; set; }

        public virtual Persona Persona { get; set; }

        public virtual Publicacion Publicacion { get; set; }
    }
}