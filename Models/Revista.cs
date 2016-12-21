using System.Collections.Generic;
namespace Publicaciones.Models
{
     public class Revista
     {
         /// <summary>
         /// The identifier of the class
         /// </summary>
         public string RevistaId { get; set; }

         /// <summary>
         /// The name of the magazine
         /// </summary>
         public string Nombre { get; set; }

         /// <summary>
         /// This is the Intenational Standard Serial Number
         /// </summary>
         public string Issn { get; set; }

         /// <summary>
         /// List that store the impact factors of the magazine
         /// </summary>
         public virtual List < Impacto > Impactos { get; set; }
     }
}