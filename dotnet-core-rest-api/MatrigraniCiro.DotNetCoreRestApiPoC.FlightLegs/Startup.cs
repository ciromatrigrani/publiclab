using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Context;
using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Repository;
using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs
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
            services.AddDbContext<FlightMeshDBContext>(opt => opt.UseInMemoryDatabase("flight_mesh_db"));
            services.AddAutoMapper(mapper => mapper.AddMaps(typeof(Mapping).Assembly));
            services.AddTransient<IFlightLegService, FlightLegService>();
            services.AddTransient<IFlightLegRepository, FlightLegRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "PoC DotNet Core Rest API for Flight Mesh control",
                Description = "A dot NET 5 (Core) API PoC made by Ciro Matrigrani",
                Contact = new OpenApiContact
                {
                    Name = "Ciro Matrigrani",
                    Email = "ciromatrigrani@gmail.com",
                    Url = new Uri("https://sites.google.com/site/cmatripgita/")
                }
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs. v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
