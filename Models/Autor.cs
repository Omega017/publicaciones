namespace Publicaciones.Models
{
    public class Autor
    {
        public string Id { get; set; }
        public Persona Persona { get; set; }

        public Publicacion Publicacion { get; set; }
    }
}