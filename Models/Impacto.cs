using System;
namespace Publicaciones.Models
{
     public class Impacto
     {
         public string ImpactoId { get; set; }

         public Q Q { get; set; }

         public string Jif { get; set; }

         public DateTime Fecha { get; set; }

         public string RevistaId { get; set; }
         public virtual Revista Revista { get; set; }

         public string IndiceId { get; set; }
         public virtual Indice Indice { get; set; }
     }

     public enum Q { Q1, Q2, Q3, Q4 };
}     