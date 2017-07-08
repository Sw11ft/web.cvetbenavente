using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using web.cvetbenavente.Data;
using web.cvetbenavente.Models;
using web.cvetbenavente.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace web.cvetbenavente
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Restrições da Password
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            });

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            //Requer HTTPS
            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            /*
            var options = new RewriteOptions()
                .AddRedirectToHttps();
            */

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();


            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "ajuda",
                    template: "Ajuda",
                    defaults: new { controller = "Home", action = "Ajuda" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "login",
                    template: "Login",
                    defaults: new { controller = "Account", action = "Login" });

                routes.MapRoute(
                    name: "errors",
                    template: "Errors/{StatusCode}",
                    defaults: new { controller = "Errors", action = "Index" });

                routes.MapRoute(
                    name: "changepassword",
                    template: "AlterarPassword",
                    defaults: new { controller = "Manage", action = "ChangePassword" });

                routes.MapRoute(
                    name: "clientedetails",
                    template: "Clientes/{id}",
                    defaults: new { controller = "Clientes", action = "Detalhes" });

                routes.MapRoute(
                    name: "clienteedit",
                    template: "Clientes/{id}/Editar",
                    defaults: new { controller = "Clientes", action = "Editar" });

                routes.MapRoute(
                    name: "animaisdetails",
                    template: "Animais/{id}",
                    defaults: new { controller = "Animais", action = "Detalhes" });

                routes.MapRoute(
                    name: "animaisedit",
                    template: "Animais/{id}/Editar",
                    defaults: new { controller = "Animais", action = "Editar" });

                routes.MapRoute(
                    name: "animaisespecie",
                    template: "Animais/Especies/{action=Index}/{id?}",
                    defaults: new { controller = "Especies" });
            });
        }
    }
}
