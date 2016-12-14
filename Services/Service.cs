using System; 
using System.Collections.Generic; 
using System.Linq; 
using Microsoft.Extensions.Logging; 
using Publicaciones.Backend; 
using Publicaciones.Models; 

namespace Publicaciones.Service {

    /// <summary>
    /// Metodos de la interface
    /// </summary>
    public interface IMainService {
        void Add(Persona persona); 

        void Add(Publicacion pub);

        void Add(Autor author);

        List < Persona > FindPersonas(string nombre);

        List <Persona> Personas();

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

        public List < Persona > FindPersonas(string nombre) {
            return BackendContext.Personas
                .Where(p => p.Nombre.Contains(nombre))
                .OrderBy(p => p.Nombre)
                .ToList(); 
        }

        public List<Persona> Personas() {
            return BackendContext.Personas.ToList();
        }

        public List<Publicacion> Publicaciones (string rut) {
            List < Autor > autores = BackendContext.Autores
                .Where(a => a.Persona.Rut.Contains(rut))
                .ToList();
            List < Publicacion > publicacionesAutor = new List< Publicacion >();
            foreach (Autor autor in autores)
            {
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
            persona.Rut = "123456789";
            persona.Nombre = "Diego"; 
            persona.Apellido = "Urrutia"; 

            // Agrego la persona al backend
            this.Add(persona); 

            // Creo la primera publicacion
            Publicacion pub = new Publicacion();
            pub.Id = "1";

            // Agrego la publicacion al backend
            this.Add(pub);

            // Creo primer autor
            Autor autor = new Autor();
            autor.Id = "1";
            autor.Persona = persona;
            autor.Publicacion = pub;

            // Agrego el autor al backend
            this.Add(autor);            

            foreach (Persona p in BackendContext.Personas) {
                Logger.LogDebug("Persona: {0}", p); 
            }

            Initialized = true;

            Logger.LogDebug("Inicializacion terminada :)");
        }
    }

}