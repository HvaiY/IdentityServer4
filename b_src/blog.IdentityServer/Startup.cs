using IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           // var builder = services.AddIdentityServer(option =>
           //{
           //    option.EmitStaticAudienceClaim = true;
           //})
           // .AddInMemoryIdentityResources(Config.IdentityResources)
           // .AddInMemoryApiScopes(Config.ApiScopes)
           // .AddInMemoryClients(Config.Clients)
           // .AddTestUsers(TestUsers.Users);

           // //builder.AddSigningCredential(); // ÃÜÔ¿²ÄÁÏ
           // // not recommended for production - you need to store your key material somewhere secure
           // builder.AddDeveloperSigningCredential();

           var cors= Configuration.GetSection("Cors");
        var logger= LoggerFactory.Create(config =>
            {
                config.AddConsole();
                config.AddDebug();
            }).CreateLogger<Startup>(); 
            logger.LogInformation(cors.Value);

            services.AddScoped<Config>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "blog.IdentityServer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration config,ILogger<Config> loger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "blog.IdentityServer v1"));
            }
            new Config(config, loger).Test();
            app.UseHttpsRedirection();
            

            app.UseRouting();
            //app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
