namespace Publicaciones.Models
{
    public class Afiliacion
    {
        public string AfiliacionId { get; set; }

        public string Nombre { get; set; }

        public string Sede { get; set; }

        public string Facultad { get; set; }

        public string Unidad { get; set; }

        public string Ubicacion { get; set; }

        public string PersonaId { get; set ;}
        public Persona Persona { get; set; }
    }
}