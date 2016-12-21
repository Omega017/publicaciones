using System;
namespace Publicaciones.Models
{
     public class Impacto
     {
         /// <summary>
         /// ImpactoId is the identifier of this class
         /// </summary>
         public string ImpactoId { get; set; }

         /// <summary>
         /// The Q is one of the Impact Factors
         /// </summary>
         public Q Q { get; set; }

         /// <summary>
         /// Is the Journal Impact Factor
         /// </summary>
         public string Jif { get; set; }

         /// <summary>
         /// The date that the Revista had this Impacto
         /// </summary>
         public DateTime Fecha { get; set; }

         /// <summary>
         /// RevistaId, identifier that reference the Revista with this Impact
         /// </summary>
         public string RevistaId { get; set; }

         /// <summary>
         /// Revista is the navigation attribute from Revista
         /// </summary>
         public virtual Revista Revista { get; set; }

         /// <summary>
         /// IndiceId, identifier that reference the Indice with this Impact
         /// </summary>
         public string IndiceId { get; set; }

         /// <summary>
         /// Indice, is the navigation attribute from Indice
         /// </summary>
         public virtual Indice Indice { get; set; }
     }

     /// <summary>
     /// It is the enumeration that use the Q of this class
     /// </summary>
     public enum Q { Q1, Q2, Q3, Q4 };
}     