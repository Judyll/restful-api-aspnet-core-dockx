using AutoMapper;
using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CourseLibrary.API
{
    public class Startup
    {
        /*In the constructor, the configuration object is injected that allows us to access
         configuration settings like configurations that we might store in appsettings.json.*/
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /*Used to add services to the build-in dependency injection container and to configure those
         services. Services should be seen as a broad concept. A service is a component that is intended
         for a common consumption in an app. The service we add here can later be injected into other
         pieces of code that live in our application*/
        public void ConfigureServices(IServiceCollection services)
        {
            /*AddControllers only register those services that are typically required when building
             APIs, like support for controllers, model, binding, data annotations and formatters. We
             are still registering MVC related services but we are just skipping the things we don't
             need.
            
             The AddControllers method accepts an action to configure it.*/
            services.AddControllers(setupAction => 
            {
                /*If this is set to false, the API will return responses in a default
                 supported format (JSON) if an unsupported media type is requested (ex. XML).*/
                setupAction.ReturnHttpNotAcceptable = true;

                /*If you are wondering how ASP.NET Core chooses its default formatter
                 if no "Accept" header is added to the request, its the first one in the
                list. */
                //setupAction.OutputFormatters.Add(
                //    new XmlDataContractSerializerOutputFormatter());

                /*In CORE 2.2 and 3, the preferred way of adding input and output formatters
                 for XML is by calling AddXmlDataContractSerializerFormatters*/
            }).AddXmlDataContractSerializerFormatters();

            /* This method allows us to input the set of assemblies. It's these assemblies
             * that will automatically scanned profiles that contain mapping configurations.
             * By calling GetAssemblies, we are adding loading profiles from all assemblies
             * in the current domain. A profile is just actually a neat way to nicely organize
             * our mapping configuration.
             */
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();

            services.AddDbContext<CourseLibraryContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=CourseLibraryDB;Trusted_Connection=True;");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /*Use the services that are registered and configured in the ConfigureServices method, so this is
          called after that. The Configure method is used to specify how an ASP.NET application will respond
          to individual HTTP requests. In other words, through this, we can configure the request pipeline.

          Each request travels through all pieces of middleware we add here in order, and each piece of the
          middleware can potentially short-circuit the request so it doesn't pass to through the next one.
          The order is very important. For example, if you add authorization middleware (UseAuthorization)
          after the controllers (UseEndpoints), the request will travel first through the controllers and
          potentially execute the code that is in there even if authorization is required which is obviously
          a bad thing.*/
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                /*When working on the developer environment, we are shown a developer-friendly page.*/
                app.UseDeveloperExceptionPage();
            }

            /*Marks the position in the middleware pipeline where a routing decision is made.
             In other words, where the endpoint is selected.*/
            app.UseRouting();

            /*Adds authorization capabilities to the request pipeline. This is important when we
             want to secure our API. You typically configured authorization in the ConfigureService
             method. If it is not configured, UseAuthorization will still allow anonymous access.*/
            app.UseAuthorization();

            /*Marks the position in the middleware pipeline where the selected endpoint
             is executed.*/
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
