using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductAPI.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace ProductAPI {
  public class Startup {

    static public IConfigurationRoot Configuration { get; set; }
    static public ConnectionStrings connectionStrings { get; set; }

    public Startup (IHostingEnvironment env) {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();

      connectionStrings = new ConnectionStrings();
      Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);
      }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices (IServiceCollection services) {
      services.AddMvc()
     .AddJsonOptions(options =>
      {
          options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      });

      var connSQL = connectionStrings.connSQL;
      services.AddDbContext<ProductLikesContext>(options => options.UseSqlServer(connSQL));

      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new Info { Title = "Product API", Version = "v1" });
      });
      }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();
      else
        app.UseExceptionHandler("/Home/Error");

      app.UseStaticFiles();

      app.UseMvc(routes => {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
      });

      app.UseSwagger();
      app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
      });
      }
    }
  }
