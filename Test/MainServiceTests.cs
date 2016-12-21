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

        [Fact]
        public void PublicacionesTest(){
            Logger.LogInformation("Testing IMainService.Publicaciones(string rut); ..");

            // Non-Existence validation
            Assert.True(Service.Publicaciones("1-9").Count == 0);

            // Invalid parameter validation
            Assert.Throws<System.ArgumentException>( () => { Service.Publicaciones("11222333-1"); });   

            // Persona Creation
            Persona persona1 = new Persona();
            persona1.Nombre = "Pancho";
            persona1.Apellido = "Villa";
            persona1.Email = "jlabra@gmail.com";
            persona1.Rut = "1-9";
            Service.Add(persona1);

            // Autor Creation
            Autor autor1 = new Autor();
            autor1.Persona = persona1;
            autor1.TipoAutor = TipoAutor.Principal;
            Service.Add(autor1);

            // New autor creation
            Autor autor2 = new Autor();
            autor2.Persona = persona1;
            autor2.TipoAutor = TipoAutor.Correspondiente;
            Service.Add(autor2);

            // Publicacion Creation
            Publicacion publicacion1 = new Publicacion();
            publicacion1.Autors = new List < Autor >();
            publicacion1.Autors.Add(autor1);
            publicacion1.FechaRevista = new DateTime(2015, 12, 11);
            publicacion1.FechaWeb = new DateTime(2016,12 ,12);
            publicacion1.PagInicio = 12;
            Service.Add(publicacion1);

            // Quantity Validation (null validation (Autor 2 without Publicacion))
            Assert.True(Service.Publicaciones("1-9").Count() == 1);

            // New Persona Creation
            Persona persona3 = new Persona();
            persona3.Nombre = "Lucho";
            persona3.Apellido = "Jara";
            persona3.Email = "ljara@gmail.com";
            persona3.Rut = "2-7";
            Service.Add(persona3);

            // New autor creation
            Autor autor3 = new Autor();
            autor3.Persona = persona3;
            autor3.TipoAutor = TipoAutor.Correspondiente;
            Service.Add(autor3);

            // Publicacion Creation
            Publicacion publicacion2 = new Publicacion();
            publicacion2.Autors = new List < Autor >();
            publicacion2.Autors.Add(autor2);
            publicacion2.Autors.Add(autor3);
            publicacion2.FechaRevista = new DateTime(2010, 12, 11);
            publicacion2.FechaWeb = new DateTime(2011,12 ,12);
            publicacion2.PagInicio = 10;
            Service.Add(publicacion2);

            // Quntity validation
            Assert.True(Service.Publicaciones("1-9").Count() == 2);
            Assert.True(Service.Publicaciones("2-7").Count() == 1);

            // Same Publicacion validation
            Publicacion publicBack1 = Service.Publicaciones("1-9").ElementAt(1);
            Publicacion publicBack2 = Service.Publicaciones("1-9").ElementAt(0);

            Assert.Equal(publicBack1.FechaRevista, publicacion2.FechaRevista);
            Assert.Equal(publicBack1.FechaWeb, publicacion2.FechaWeb);
            Assert.Equal(publicBack1.PagInicio, publicacion2.PagInicio);

            Assert.Equal(publicBack2.FechaRevista, publicacion1.FechaRevista);
            Assert.Equal(publicBack2.FechaWeb, publicacion1.FechaWeb);
            Assert.Equal(publicBack2.PagInicio, publicacion1.PagInicio);

            // Inverse navigation test
            Autor autorInverse =  Service.FindAutoresByRut("2-7").First();
            Assert.Equal(autorInverse.PublicacionId, publicBack1.PublicacionId);
            Assert.Equal(autorInverse.Publicacion.PagInicio, publicBack1.PagInicio);
            Assert.Equal(autorInverse.Publicacion.FechaRevista, publicBack1.FechaRevista);
            Assert.Equal(autorInverse.Publicacion.FechaWeb, publicBack1.FechaWeb);

            // Extensive atributes test

            Autor autor5 = new Autor();
            autor5.Persona = persona1;
            autor5.TipoAutor = TipoAutor.Correspondiente;
            Service.Add(autor5);

            Autor autor6 = new Autor();
            autor6.Persona = persona3;
            autor6.TipoAutor = TipoAutor.Correspondiente;
            Service.Add(autor6);

            Indice indice5 = new Indice();
            indice5.Nombre = "Scolopolulus";
            Service.Add(indice5);

            Impacto impacto5 = new Impacto();
            impacto5.Fecha = new DateTime(1990, 12, 12);
            impacto5.Indice = indice5;
            impacto5.Jif = "12341";
            impacto5.Q = Q.Q2;
            Service.Add(impacto5);

            Revista revista5 = new Revista();
            revista5.Nombre = "Lo que callamos los hombres";
            revista5.Issn = "12";
            revista5.Impactos = new List < Impacto >();
            revista5.Impactos.Add(impacto5);
            Service.Add(revista5);

            Paper paper4 = new Paper();
            paper4.Abstract = "This is an Abstrac abstracto";
            paper4.AreaDesarrollo = "NN";
            paper4.FechaInicio = new DateTime(2015, 10, 10);
            paper4.FechaTermino = new DateTime(2015, 11, 11);
            paper4.LineaInvestigativa = "Las ideas de la vida";
            paper4.Titulo = "Las aberraciones de ici";
            Service.Add(paper4);

            Paper paper5 = new Paper();
            paper5.Abstract = "This is an Abstrac abstracto";
            paper5.AreaDesarrollo = "NN";
            paper5.FechaInicio = new DateTime(2016, 10, 10);
            paper5.FechaTermino = new DateTime(2016, 11, 11);
            paper5.LineaInvestigativa = "Las ideas de la vida";
            paper5.Titulo = "Las aberraciones de ici";
            Service.Add(paper5);

            Publicacion publicacion5 = new Publicacion();
            publicacion5.FechaRevista = new DateTime(2016, 12, 12);
            publicacion2.FechaWeb = new DateTime(2016,12,12);
            publicacion5.PagInicio = 12;
            publicacion5.Revista = revista5;
            publicacion5.Papers = new List < Paper >();
            publicacion5.Papers.Add(paper4);
            publicacion5.Papers.Add(paper5);
            publicacion5.Autors = new List < Autor >();
            publicacion5.Autors.Add(autor5);
            publicacion5.Autors.Add(autor6);
            Service.Add(publicacion5);

            Publicacion publicBack5 = Service.Publicaciones("1-9").Last();

            // Papers verification
            Assert.True(publicBack5.Papers.Count() == 2);
            Assert.True(publicBack5.Papers.First().PaperId == paper4.PaperId);
            Assert.True(publicBack5.Papers.Last().PaperId == paper5.PaperId);

            // Autors verification
            Assert.True(publicBack5.Autors.First().AutorId == autor5.AutorId);
            Assert.True(publicBack5.Autors.Last().AutorId == autor6.AutorId);

            // Revista verification
            Assert.True(publicBack5.Revista.RevistaId == revista5.RevistaId);

            // Impacto verification
            Assert.True(publicBack5.Revista.Impactos.First().ImpactoId == impacto5.ImpactoId);

            // Indice verification
            Assert.True(publicBack5.Revista.Impactos.First().Indice.IndiceId == indice5.IndiceId);     


            Logger.LogInformation("Testing IMainService.Publicaciones(string rut); OK");
        }

        void IDisposable.Dispose()
        {
            // Aca eliminar el Service
        }
    }

}