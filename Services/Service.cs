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

        public Persona FindPersonasByRut(string rut) {
            return BackendContext.Personas
                .Where(p => p.Rut.Equals(rut)).Single();
        }

        public List < Autor > FindAutoresByRut(string rut) {
            return BackendContext.Autores
                .Where(a => a.Rut.Contains(rut))
                .OrderBy(a => a.Persona.Nombre)
                .ToList();
        }


        public List<Persona> Personas() {
            return BackendContext.Personas.ToList();
        }

        public List<Publicacion> Publicaciones (string rut) {
            List < Autor > autores = BackendContext.Autores
                .Where(a => a.Rut.Contains(rut))
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
            string rut = "123456789";
            //persona.Rut = rut;

            // Agrego la persona al backend
            this.Add(persona); 


            // *** Borrar, es solo una prueba
            // ****
            //Persona pp = FindPersonasByRut(persona.Rut);
            Persona pp = FindPersonasByRut(rut);
            Logger.LogWarning("Este es el nombre de la persona: " + pp.Nombre );
            Logger.LogWarning("Este es el apellido de la persona: " + pp.Apellido );
            Logger.LogWarning("Este es el rut de la persona: " + pp.Rut );

            Publicacion publicacion = new Publicacion();
            //pagina de inicio de la publicacion
            publicacion.PagInicio = 1;
            //id de la publicacion
            publicacion.PublicacionId="Publicacion001";
            Logger.LogWarning("Este es la id de la publicacion: " + publicacion.PublicacionId );
            BackendContext.Publicaciones.Add(publicacion); 
            BackendContext.SaveChanges();

            Publicacion publicacion2 = new Publicacion();
            //pagina de inicio de la publicacion
            publicacion2.PagInicio = 15;
            //id de la publicacion
            publicacion2.PublicacionId="Publicacion002";
            Logger.LogWarning("Este es la id de la publicacion: " + publicacion2.PublicacionId );
            BackendContext.Publicaciones.Add(publicacion2); 
            BackendContext.SaveChanges();

            //autor de la primera publicacion
            Autor autor = new Autor();
            autor.Persona = persona;
            autor.Publicacion = publicacion;
            BackendContext.Autores.Add(autor);
            BackendContext.SaveChanges();

            //autor de la segunda publicacion (el mismo)
            Autor autor2 = new Autor();
            autor2.Persona = persona;
            autor2.Publicacion = publicacion2;
            BackendContext.Autores.Add(autor2);
            BackendContext.SaveChanges();

            Logger.LogWarning("Yo soy un autor: " + FindAutoresByRut(persona.Rut).ElementAt(0).Persona.Nombre);
            Logger.LogWarning("Publicaciones pagina de Diego: " + Publicaciones(persona.Rut).ElementAt(0).PagInicio);
            Logger.LogWarning("Publicaciones pagina de Diego: " + Publicaciones(persona.Rut).ElementAt(1).PagInicio);            

            //List < Publicacion > lp = Publicaciones(persona.Rut);
            //Logger.LogWarning("Esta es la pagina de inicio de una publicacion de Diego: " + lp.ElementAt(0).PagInicio);
            // ****
            // ***


            foreach (Persona p in BackendContext.Personas) {
                Logger.LogDebug("Persona: {0}", p); 
            }

            Initialized = true;

            Logger.LogDebug("Inicializacion terminada :')");
        }
    }

}