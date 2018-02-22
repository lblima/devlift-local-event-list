using DevLiftLocalEventList.Domain.Interfaces;
using DevLiftLocalEventList.Infrastructure;
using DevLiftLocalEventList.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DevLiftLocalEventList.WebApi
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
            services.AddDbContext<LocalEventContext>(opt => opt.UseInMemoryDatabase("LocalEventDataBase"));
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventTypeRepository, EventTypeRepository>();

            services.AddAutoMapper();
            services.AddCors(options =>
            {
                options.AddPolicy("DevLiftPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            services.AddMvc();

            // Register the Swagger for api documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Local Event API for DevLift Media",
                    Description = "A simple local event ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Leonardo Lima", Email = "leonardo.bruno@outlook.com", Url = "https://www.linkedin.com/in/lblima/" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                    options =>
                     {
                         options.Run(
                         async context =>
                         {
                             context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                             context.Response.ContentType = "text/html";
                             var ex = context.Features.Get<IExceptionHandlerFeature>();
                             if (ex != null)
                             {
                                 var err = "<h1>Ops, something goes wrong :( Sorry, try again later</h1>";

                             // Log exception here... ex.Error.Message && ex.Error.StackTrace

                             await context.Response.WriteAsync(err).ConfigureAwait(false);
                             }
                         });
                     }
                    );
            }

            app.UseCors("DevLiftPolicy");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}