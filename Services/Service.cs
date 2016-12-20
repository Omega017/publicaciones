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
    /// Interface that names the methods that the MainService implements like AOP 
    /// </summary>
    public interface IMainService {

        /// <summary>
        /// This methods add a person to the backend
        /// </summary>
        /// <param name="persona"></param>
        void Add(Persona persona); 

        /// <summary>
        /// This methods add a publication to the backend
        /// </summary>
        /// <param name="pub"></param>
        void Add(Publicacion pub);

        /// <summary>
        /// This methods add an author to the backend
        /// </summary>
        /// <param name="author"></param>
        void Add(Autor author);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicacionId"></param>
        /// <param name="autor"></param>
        void AddAutorToPublicacion(string publicacionId, Autor autor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        List < Persona > FindPersonas(string nombre);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        Persona FindPersonaByRut(string rut);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        List < Autor > FindAutoresByRut(string rut);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List <Persona> Personas();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        List <Publicacion> Publicaciones (String rut);

        void Initialize(); 
    }

    /// <summary>
    /// Implementacion de la interface IMainService
    /// </summary>
    public class MainService:IMainService {

        /// <summary>
        /// Acceso al Backend
        /// </summary>
        /// <returns></returns>
        private BackendContext BackendContext { get; set; }

        /// <summary>
        /// The Logger 
        /// </summary>
        /// <returns></returns>
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

            // Obtengo el backend
            BackendContext = backendContext; 
            if (BackendContext == null) {
                throw new ArgumentNullException("MainService requiere del BackendService != null"); 
            }

            // No se ha inicializado
            Initialized = false;

            Logger.LogInformation("MainService created"); 
        }

        /// <summary>
        /// Agregar persona al backend
        /// </summary>
        /// <param name="persona"></param>
        public void Add(Persona persona) {

            // Guardo la Persona en el Backend
            BackendContext.Personas.Add(persona); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }

        /// <summary>
        /// Agregar publicacion al backend
        /// </summary>
        /// <param name="publi"></param>
        public void Add(Publicacion publi){
            // Guardo la publicacion en el Backend
            BackendContext.Publicaciones.Add(publi);
            // Guardar cambios
            BackendContext.SaveChanges();
        }

        /// <summary>
        /// Agregar autor al backend
        /// </summary>
        /// <param name="author"></param>
        public void Add(Autor author){
            // Guardo la publicacion en el Backend
            BackendContext.Autores.Add(author);
            // Guardar cambios
            BackendContext.SaveChanges();
        }

        public void AddAutorToPublicacion(string publicacionId, Autor autor){
            BackendContext.Publicaciones
                .Where(p => p.PublicacionId == publicacionId)
                .First().Autors.Add(autor);
            BackendContext.SaveChanges();
        }

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
        /// <returns></returns>
        public Persona FindPersonaByRut(string rut) {
            if(!ModelValidations.isValidRut(rut)) {
                throw new System.ArgumentException("Incorrect verified rut's number","rut");
            }
            return BackendContext.Personas
                .Where(p => p.Rut.Equals(rut)).SingleOrDefault();
        }

        public List < Autor > FindAutoresByRut(string rut) {
            return BackendContext.Autores
                .Include(a => a.Publicacion)
                .Include(a => a.Persona)
                .Where(a => a.Rut.Contains(rut))
                .OrderBy(a => a.Persona.Nombre)
                .ToList();
        }


        public List<Persona> Personas() {
            return BackendContext.Personas.ToList();
        }

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