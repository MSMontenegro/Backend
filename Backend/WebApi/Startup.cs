using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Backend.Business.DTO.Mappers.Interfaces;
using Backend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Backend.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:53000");
                });
            });

            services.AddControllers();
            AddSwagger(services);
            
            var builder = new ContainerBuilder();

            builder.Populate(services);

            RegisterAllDependencies(builder, "Urus");

            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Backend {groupName}",
                    Version = groupName,
                    Description = "Microservicio de Backend",
                    Contact = new OpenApiContact
                    {
                        Name = "MMontenegro Company",
                        Email = string.Empty,
                        Url = new Uri("https://foo.com/"),
                    }
                });
            });
        }

        private void RegisterAllDependencies(ContainerBuilder builder, string assembliesname)
        {
            var assemblies = GetAssemblies(assembliesname);

            builder
                .RegisterAssemblyTypes(assemblies)
                .AssignableTo(typeof(IWeatherForecastService))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(assemblies)
                .AssignableTo(typeof(IWeatherForecastMapper))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        private static Assembly[] GetAssemblies(string assembliesname = "Backend")
        {
            var assemblies =
                DependencyContext
                    .Default
                    .GetDefaultAssemblyNames()
                    .Where(e => e.FullName.Contains(assembliesname) || e.FullName.Contains("Backend"))
                    .Select(Assembly.Load)
                    .ToArray();
            return assemblies;
        }
    }
}
