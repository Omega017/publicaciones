using Microsoft.EntityFrameworkCore; 
using Publicaciones.Models; 

namespace Publicaciones.Backend {

    /// <summary>
    /// Representation of the Database
    /// </summary>
    public class BackendContext : DbContext {

        /// <summary>
        /// Empty constructor for the tests
        /// </summary>
        public BackendContext() {
            
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        public BackendContext(DbContextOptions < BackendContext > options) : base(options) {
        }

        /// <summary>
        /// Backend people representation
        /// </summary>
        /// <returns> Link to the DB of the people</returns>
        public DbSet < Persona > Personas { get; set; }

        /// <summary>
        /// Backend publication representation
        /// </summary>
        /// <return> Link to the DB of the publications</return>
        public DbSet < Publicacion > Publicaciones { get; set; }
         
         /// <summary>
         /// Backend authors representation
         /// </summary>
         /// <return> Link to the DB of the authors</return>
        public DbSet < Autor > Autores { get; set; }

        /// <summary>
        /// Backend magazines reresentation
        /// </summary>
        /// <returns> Link to the DB of the magazines</returns>
        public DbSet < Revista > Revistas { get; set; }

        /// <summary>
        /// Backend indexes reresentation
        /// </summary>
        /// <returns> Link to the DB of the indexes</returns>
        public DbSet < Indice > Indices { get; set; }

        /// <summary>
        /// Backend impacts representation
        /// </summary>
        /// <returns> Link to the DB of the impacts</returns>
        public DbSet < Impacto > Impactos { get; set; }

        /// <summary>
        /// Backend papers representation
        /// </summary>
        /// <returns> Link to the DB of the papers</returns>
        public DbSet < Paper > Papers { get; set; }
    }

}