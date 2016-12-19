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
            Service.Initialize();
            
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