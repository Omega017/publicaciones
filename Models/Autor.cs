namespace Publicaciones.Models
{
    public class Autor
    {
        public string Id { get; set; }
        public Persona persona { get; set; }

        public Publicacion publicacion { get; set; }
    }
}