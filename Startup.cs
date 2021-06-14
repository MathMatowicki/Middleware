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
using Microsoft.AspNetCore.Http;

namespace Middleware
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
            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();



            app.Use(async (context, next) =>
            {
                //logic on request
                await context.Response.WriteAsync("<p>hello midd1</p>");
                await next();
                //logic on response
            });

            app.Use(async (context, next) =>
            {
                //logic on request
                await context.Response.WriteAsync("<p>First</p>");
                await next();
                //logic on response
            });
            app.Map("/angular", action =>
            {
                action.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("<p>Angular 6</p>");
                    await next();
                });
                action.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("<p>was released</p>");
                    await next();
                });
                action.Run(async context =>
                {
                    await context.Response.WriteAsync("<p>May 3rd 2018</p>");
                });
            });

            app.MapWhen(context => context.Request.Query.ContainsKey("name"), action =>
            {
                action.Run(async context =>
                {
                    var name = context.Request.Query["name"];
                    await context.Response.WriteAsync($"<h1>Hello {name}</h1>");
                });
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("<p>hello RUN non map midd</p>");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
