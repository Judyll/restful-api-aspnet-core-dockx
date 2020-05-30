using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CourseLibrary.API
{
    public class Program
    {
        /*Starting point of our application. The Main method here is responsible for
         the configuring and running the app. In our case, we are running the web application
         that needs to be hosted. That is what is created by the CreateHostBuilder method.*/
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    /*We are using the Startup class as the startup type.*/
                    webBuilder.UseStartup<Startup>();
                });
    }
}
