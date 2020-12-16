using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;

namespace MvcClient
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
            services.AddControllersWithViews();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddAuthentication(options =>
                      {
                          options.DefaultScheme = "Cookies";
                          options.DefaultChallengeScheme = "oidc";
                      })
                  .AddCookie("Cookies")
                  .AddOpenIdConnect("oidc", options =>
                  {
                      // 跳转到 identityServer 登录(alice alice) 然后跳转回指定页面 需要在id4里面配置
                      options.Authority = "https://localhost:5001";
                      options.ClientId = "mvc";
                      options.ClientSecret = "secret";
                      options.ResponseType = "code";

                      options.SaveTokens = true;
                      
                      //// 请求范围控制
                      //options.Scope.Add("profile");
                      //options.GetClaimsFromUserInfoEndpoint = true;
                      //// userinfo 与claimAction 映射
                      //options.ClaimActions.MapUniqueJsonKey("myclaim1", "myclaim1");

                      options.Scope.Add("api1");
                      options.Scope.Add("offline_access");
                  });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .RequireAuthorization(); // 必须授权 禁用匿名访问
            });
        }
    }
}
