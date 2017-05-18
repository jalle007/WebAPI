using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace ProductAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

          /*
            * Server=tcp:productapi20170517052226dbserver.database.windows.net;Database=ProductAPI20170517052226_db;
User ID=jaskobh@hotmail.com;Password=Tibra2016&;Trusted_Connection=False;
Encrypt=True;
            * */

        var connection = @"Server=tcp:productapi20170517052226dbserver.database.windows.net;Database=ProductAPI20170517052226_db;
User ID=jalle;Password=keyboard123!;Trusted_Connection=False;Encrypt=True;";
            //var connection = @"Data Source=mylaptop;Initial Catalog=ProductLikes;Integrated Security=True";
            services.AddDbContext<ProductLikesContext>(options => options.UseSqlServer(connection));

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Info { Title = "Product API", Version = "v1" });
        }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

              if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

           app.UseSwagger();
           app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
            });
        }
    }
}
