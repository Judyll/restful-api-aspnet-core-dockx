using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
             need.*/
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
       /*Use the services that are registered and configured in the ConfigureServices method, so this is
         called after that. The Configure method is used to specify how an ASP.NET application will respond
         to individual HTTP requests. In other words, through this, we can configure the request pipeline.
         
         Each request travels through all pieces of middleware we add here in order, and each piece of the
         middleware can potentially short-circuit the request so it doesn't pass to through the next one.*/
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                /*When working on the developer environment, we are shown a developer-friendly page.*/
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            /*Adds authorization capabilities to the request pipeline. This is important when we
             want to secure our API.*/
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
