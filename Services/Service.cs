using System; 
using System.Collections.Generic; 
using Microsoft.EntityFrameworkCore;
using System.Linq; 
using Microsoft.Extensions.Logging; 
using Publicaciones.Backend; 
using Publicaciones.Models; 
using Utilities.Validations;

namespace Publicaciones.Service {

    /// <summary>
    /// This interface names the methods that the MainService implements like AOP 
    /// </summary>
    public interface IMainService {

        /// <summary>
        /// This method add a person to the backend
        /// </summary>
        /// <param name="persona">Persona must be valid and not null</param>
        void Add(Persona persona); 

        /// <summary>
        /// This method add a publication to the backend
        /// </summary>
        /// <param name="pub">Publication must be valid and not null</param>
        void Add(Publicacion pub);

        /// <summary>
        /// This method add an author to the backend
        /// </summary>
        /// <param name="author">Author must be valid and not null</param>
        void Add(Autor author);

        /// <summary>
        /// This method add a magazine to te backend
        /// </summary>
        /// <param name="revista">Revista must be valid and not null</param>
        void Add(Revista revista);

        /// <summary>
        /// This method add an impact to the backend
        /// </summary>
        /// <param name="impacto">Impacto must be valid and not null</param>
        void Add(Impacto impacto);

        /// <summary>
        /// This method add and indice to the backend
        /// </summary>
        /// <param name="indice">Indice must be valid and not null</param>
        void Add(Indice indice);

        /// <summary>
        /// This method add a paper to the backend
        /// </summary>
        /// <param name="paper">Paper must be valid and not null</param>
        void Add(Paper paper);

        /// <summary>
        /// This method join the author with the publication
        /// </summary>
        /// <param name="publicacionId">publicacionId must be valid and not null, a number  </param>
        /// <param name="autor">Autor must be valid and not null, it should have all his attributes </param>
        void AddAutorToPublicacion(string publicacionId, Autor autor);

        /// <summary>
        /// This method put in a list, all people with name in the parameter 
        /// </summary>
        /// <param name="nombre">nombre must be valid, it shoud have only letters</param>
        /// <returns>List of Personas related to the entered rut</returns>
        List < Persona > FindPersonas(string nombre);

        /// <summary>
        /// This method finds a person with the parameter rut 
        /// </summary>
        /// <param name="rut">Rut must be valid, it must have numbers, followed by dash, and one number or K </param>
        /// <returns>This return just one person related to the rut</returns>
        Persona FindPersonaByRut(string rut);

        /// <summary>
        /// This method put in a list authors with the parameter rut
        /// </summary>
        /// <param name="rut">Rut must be valid, it must have numbers, followed by dash, and one number or K </param>
        /// <returns>List of authors related to the entered rut</returns>
        List < Autor > FindAutoresByRut(string rut);

        /// <summary>
        /// This method puts in a list of Persona, all people on the backend
        /// </summary>
        /// <returns>All people on the backend as list of Personas</returns>
        List <Persona> Personas();

        /// <summary>
        /// This method finds all publications of the parameter rut
        /// </summary>
        /// <param name="rut">Rut must be valid, it must have numbers, followed by dash, and one number or K</param>
        /// <returns>List of publications </returns>
        List <Publicacion> Publicaciones (String rut);

        void Initialize(); 
    }

    /// <summary>
    /// Implementation of the interface IMainService
    /// </summary>
    public class MainService:IMainService {

        /// <summary>
        /// Access to the backend
        /// </summary>
        /// <returns>BackendContext that contain the index to backend</returns>
        private BackendContext BackendContext { get; set; }

        /// <summary>
        /// The Logger 
        /// </summary>
        /// <returns>ILogger</returns>
        private ILogger Logger { get; set; }

        private Boolean Initialized { get; set; }

        /// <summary>
        /// Constructor via DI
        /// </summary>
        /// <param name="backendContext"></param>
        /// <param name="loggerFactory"></param>
        public MainService(BackendContext backendContext, ILoggerFactory loggerFactory) {

            // Inicializacion del Logger
            Logger = loggerFactory?.CreateLogger < MainService > (); 
            if (Logger == null) {
                throw new ArgumentNullException(nameof(loggerFactory)); 
            }

            // Obtain the backend
            BackendContext = backendContext; 
            if (BackendContext == null) {
                throw new ArgumentNullException("MainService requiere del BackendService != null"); 
            }

            // Not initialized  
            Initialized = false;

            Logger.LogInformation("MainService created"); 
        }

        /// <summary>
        /// Implementation that add a person to the backend
        /// </summary>
        /// <param name="persona"></param>
        public void Add(Persona persona) {

            // Guardo la Persona en el Backend
            BackendContext.Personas.Add(persona); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }

        /// <summary>
        /// Implementation that add a publication to the backend
        /// </summary>
        /// <param name="publi"></param>
        public void Add(Publicacion publi){
            // Saving the publication to the backend
            BackendContext.Publicaciones.Add(publi);
            // Save Changes
            BackendContext.SaveChanges();
        }

        /// <summary>
        /// Implementation that add an author to the backend
        /// </summary>
        /// <param name="author"></param>
        public void Add(Autor author){
            //Saving the author to the backend
            BackendContext.Autores.Add(author);
            // Save Changes
            BackendContext.SaveChanges();
        }

        public void Add(Revista revista) {
            // Saving the magazine to the backend
            BackendContext.Revistas.Add(revista);
            // Save Changes
            BackendContext.SaveChanges();
        }

        public void Add(Impacto impacto) {
            // Saving the impact to the backend
            BackendContext.Impactos.Add(impacto);
            // Save Changes
            BackendContext.SaveChanges();
        }

        public void Add(Paper paper) {
            // Saving the paper to the backend
            BackendContext.Papers.Add(paper);
            // Save Changes
            BackendContext.SaveChanges();
        }

        public void Add(Indice indice) {
            // Saving the index to the backend
            BackendContext.Indices.Add(indice);
            // Save Changes
            BackendContext.SaveChanges();
        }

        /// <summary>
        /// Link the autor with the publication through the publicationId and Autor as instance of a class
        /// </summary>
        /// <param name="publicacionId">The publicationId must be valid, just numbers</param>
        /// <param name="autor">Autor must be valid and should be an instance of the class with all params</param>
        public void AddAutorToPublicacion(string publicacionId, Autor autor){
            BackendContext.Publicaciones
                .Where(p => p.PublicacionId == publicacionId)
                .First().Autors.Add(autor);
            BackendContext.SaveChanges();
        }

        /// <summary>
        /// Finds all people on the backend that contain the entered string
        /// </summary>
        /// <param name="nombre">Nombre must be valid, just letters</param>
        /// <returns>List of Persona</returns>
        public List < Persona > FindPersonas(string nombre) {
            return BackendContext.Personas
                .Where(p => p.Nombre.Contains(nombre))
                .OrderBy(p => p.Nombre)
                .ToList(); 
        }

        /// <summary>
        /// Find persons by rut.
        /// </summary>
        /// <param name="rut">Without dots, with verification number and dash e.g. 11502391-5</param>
        /// <returns>Persona</returns>
        public Persona FindPersonaByRut(string rut) {
            if(!ModelValidations.isValidRut(rut)) {
                throw new System.ArgumentException("Incorrect verified rut's number","rut");
            }
            return BackendContext.Personas
                .Where(p => p.Rut.Equals(rut)).SingleOrDefault();
        }

        /// <summary>
        /// Find authors by rut
        /// </summary>
        /// <param name="rut">Without dots, with verification number and dash e.g. 11502391-5</param>
        /// <returns>List of Autor</returns>
        public List < Autor > FindAutoresByRut(string rut) {
            return BackendContext.Autores
                .Include(a => a.Publicacion)
                .Include(a => a.Persona)
                .Where(a => a.Rut.Contains(rut))
                .OrderBy(a => a.Persona.Nombre)
                .ToList();
        }

        /// <summary>
        /// This method return all people on the backend
        /// </summary>
        /// <returns>List of Persona</returns>
        public List<Persona> Personas() {
            return BackendContext.Personas.ToList();
        }

        /// <summary>
        /// This method return the publications that contain the entered rut 
        /// </summary>
        /// <param name="rut">Without dots, with verification number and dash e.g. 11502391-5</param>
        /// <returns>List of Publicacion</returns>
        public List<Publicacion> Publicaciones (string rut) {
            if(!ModelValidations.isValidRut(rut)) {
                throw new System.ArgumentException("Incorrect verified rut's number","rut");
            }
            List < Autor > autores = this.FindAutoresByRut(rut);
            List < Publicacion > publicacionesAutor = new List< Publicacion >();
            foreach (Autor autor in autores) {
                if(autor.Publicacion != null)
                    publicacionesAutor.Add(autor.Publicacion);
            }
            return publicacionesAutor;
        }

        /// <summary>
        /// This method return all publications on the backend
        /// </summary>
        /// <returns>List of Publicacion</returns>
        public List<Publicacion> Publicaciones(){
            List< Publicacion > publicaciones = new List < Publicacion >();
            foreach (Publicacion pub in publicaciones){
                publicaciones.Add(pub);
            }
            return publicaciones;
        }

        /// <summary>
        /// This method is a Singleton that is called just once
        /// </summary>
        public void Initialize() {

            if (Initialized) {
                throw new Exception("Solo se permite llamar este metodo una vez");
            }

            Logger.LogDebug("Realizando Inicializacion .."); 
            // Persona por defecto
            Persona persona = new Persona(); 
            persona.Rut = "1-9";
            persona.Nombre = "Diego"; 
            persona.Apellido = "Urrutia"; 

            // Agrego la persona al backend
            this.Add(persona);             

            foreach (Persona p in BackendContext.Personas) {
                Logger.LogDebug("Persona: {0}", p); 
            }

            Initialized = true;

            Logger.LogDebug("Inicializacion terminada :')");
        }
    }

}