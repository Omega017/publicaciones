using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Publicaciones.Backend;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Publicaciones.Models;

namespace Publicaciones.Service {

    public class MainServiceTest : IDisposable
    {
        IMainService Service { get; set; }

        BackendContext Back { get; set; }


        ILogger Logger { get; set; }

        public MainServiceTest()
        {
            // Logger Factory
            ILoggerFactory loggerFactory = new LoggerFactory().AddConsole().AddDebug();
            Logger = loggerFactory.CreateLogger<MainServiceTest>();

            Logger.LogInformation("Initializing Test ..");

            // SQLite en memoria
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Opciones de inicializacion del BackendContext
            var options = new DbContextOptionsBuilder<BackendContext>()
            .UseSqlite(connection)
            .Options;

            // inicializacion del BackendContext
            BackendContext backend = new BackendContext(options);
            // Crear la base de datos
            backend.Database.EnsureCreated();

            // Servicio a probar
            Service = new MainService(backend, loggerFactory);

            Logger.LogInformation("Initialize Test ok.");
        }


        [Fact]
        public void InitializeTest()
        {
            Logger.LogInformation("Testing IMainService.Initialize() ..");
            Service.Initialize();

            // No se puede inicializar 2 veces
            Assert.Throws<Exception>( () => { Service.Initialize(); });

            // Personas en la BD
            List<Persona> personas = Service.Personas();

            // Debe ser !=  de null
            Assert.True(personas != null);

            // Debe haber solo 1
            Assert.True(personas.Count == 1);

            // Print de la persona
            foreach(Persona persona in personas) {
                Logger.LogInformation("Persona: {0}", persona);
            }
            {
                //Testing metodo Publicaciones
                //Logger.LogInformation("Testing: Probando Metodo Publicaciones");
                //Persona persona = personas.First();
                //TestPublicacionesAutor(persona.Rut);
            }
            

            Logger.LogInformation("Test IMainService.Initialize() ok");
        }

        [Fact]
        public void FindPersonaByRutTest()
        {   
            Logger.LogInformation("Testing IMainService.FindPersonaByRut(string rut); ..");

            // Test Person.
            Persona persona = new Persona();
            persona.Nombre = "Alfredo";
            persona.Apellido = "Henriquez";
            persona.Email = "alfred@sodired.com";
            persona.Rut = "17725104-6";

            // Person added to bd.
            Service.Add(persona);

            // Getting person from bd.
            Persona backPersona = Service.FindPersonaByRut(persona.Rut);

            // Existence validation.
            Assert.NotNull(backPersona);

            // Parameter validation
            Assert.True(persona.Rut == backPersona.Rut);
            Assert.True(persona.Nombre == backPersona.Nombre);
            Assert.True(persona.Apellido == backPersona.Apellido);
            Assert.True(persona.Rut == backPersona.Rut);

            // non-existence validation.
            Assert.Null(Service.FindPersonaByRut("18508182-6"));

            // Invalid parameter validation
            Assert.Throws<System.ArgumentException>( () => { Service.FindPersonaByRut("11222333-1"); });     

            Logger.LogInformation("Test IMainService.FindPersonaByRut(string rut); OK");
            
        }

        [Fact]
        public void FindAutoresByRutTest() {
            Logger.LogInformation("Testing IMainService.FindAutoresByRut(string rut); ..");

            // Non-existence validation
            Assert.True(Service.FindAutoresByRut("1-9").Count() == 0);

            // Person Creation
            Persona persona1 = new Persona();
            persona1.Nombre = "Juan Antonio";
            persona1.Apellido = "Labra";
            persona1.Email = "jlabra@gmail.com";
            persona1.Rut = "1-9";
            Service.Add(persona1);

            // Autor Creation
            Autor autor1 = new Autor();
            autor1.Persona = persona1;
            autor1.TipoAutor = TipoAutor.Principal;
            Service.Add(autor1);

            // Autor existence validation
            Assert.NotNull(Service.FindAutoresByRut("1-9"));

            // Publicacion Creation
            Publicacion publicacion1 = new Publicacion();
            publicacion1.Autors = new List < Autor >();
            publicacion1.Autors.Add(autor1);
            publicacion1.FechaRevista = new DateTime(2015, 12, 11);
            publicacion1.FechaWeb = new DateTime(2016,12 ,12);
            publicacion1.PagInicio = 12;
            Service.Add(publicacion1);

            // At this time, service has only one Autor
            Autor autorBack = Service.FindAutoresByRut(persona1.Rut).First();
            
            // Autor data validation
            Assert.True(autorBack.TipoAutor == autor1.TipoAutor);

            // Person data validation
            Assert.True(autorBack.Persona.Rut == persona1.Rut);
            Assert.True(autorBack.Persona.Nombre == persona1.Nombre);
            Assert.True(autorBack.Persona.Apellido == persona1.Apellido);
            Assert.True(autorBack.Persona.Email == persona1.Email);

            // Publicacion data validation (Inverse Navigation)
            Assert.True(autorBack.PublicacionId == publicacion1.PublicacionId);
            Assert.True(autorBack.Publicacion.FechaWeb == publicacion1.FechaWeb);
            Assert.True(autorBack.Publicacion.FechaRevista == publicacion1.FechaRevista);
            Assert.True(autorBack.Publicacion.PagInicio == publicacion1.PagInicio);

            // New autor creation
            Autor autor2 = new Autor();
            autor2.Persona = persona1;
            autor2.TipoAutor = TipoAutor.Correspondiente;
            Service.Add(autor2);

            // Quatinty validation
            Assert.True(Service.FindAutoresByRut(persona1.Rut).Count() == 2);

            // Same Person data validation
            autorBack = Service.FindAutoresByRut(persona1.Rut).ElementAt(0);
            Autor autorBack2 = Service.FindAutoresByRut(persona1.Rut).ElementAt(1);
            Assert.True(autorBack.Persona.Rut == autorBack2.Persona.Rut);
            Assert.True(autorBack.Persona.Nombre == autorBack2.Persona.Nombre);
            Assert.True(autorBack.Persona.Apellido == autorBack2.Persona.Apellido);
            Assert.True(autorBack.Persona.Email == autorBack2.Persona.Email);

            Logger.LogInformation("Test IMainService.FindAutoresByRut(string rut); OK");
            
        }

        

        /*
        [Theory,InlineData("1")]
        public void TestPublicacionesAutor(string rut){
            Logger.LogInformation("Iniciando TestPublicacionesAutor...");
            Service.Initialize();
            List <Publicacion> publicacionesPorAutor = Service.Publicaciones(rut);
            Assert.NotNull(publicacionesPorAutor.First());
            Logger.LogInformation("Testing Metodo Publicaciones: " + publicacionesPorAutor.First().PublicacionId);
        }
        */

        void IDisposable.Dispose()
        {
            // Aca eliminar el Service
        }
    }

}