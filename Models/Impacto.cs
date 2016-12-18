using System;
namespace Publicaciones.Models
{
     public class Impacto
     {
         public string ImpactoId { get; set; }

         public string Q { get; set; }

         public string Jif { get; set; }

         public DateTime Fecha { get; set; }

         public string RevistaId { get; set; }
         public virtual Revista Revista { get; set; }

         public string IndiceId { get; set; }
         public virtual Indice Indice { get; set; }
     }
}     