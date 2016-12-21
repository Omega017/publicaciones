namespace Publicaciones.Models
{
    public class Contrato
    {
        public string ContratoId { get; set; }

        public string Cargo { get; set; }

        public string AfiliacionId { get; set; }
        public Afiliacion Afiliacion { get; set; }

        public string AutorId { get; set; }
        public Autor Autor { get; set; }

    }
}