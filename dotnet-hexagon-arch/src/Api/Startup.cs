using MatrigraniCiro.HexagonArch.BestBlogs.Application.Mapping;
using MatrigraniCiro.HexagonArch.BestBlogs.Application.Services;
using MatrigraniCiro.HexagonArch.BestBlogs.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;

namespace MatrigraniCiro.HexagonArch.BestBlogs.Api
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
            services.AddDbContext<BlogContext>(opt => opt.UseInMemoryDatabase("InMemoryDb"));
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddAutoMapper(mapper => mapper.AddMaps(typeof(Mapping).Assembly));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Ciro Matrigrani Hexagon Arch PoC BlogPosts",
                    Description = "DotNet Core Rest API for BlogPosts and Comments",
                    Contact = new OpenApiContact
                    {
                        Name = "Ciro Matrigrani",
                        Email = "ciromatrigrani@gmail.com",
                        Url = new Uri("https://sites.google.com/site/cmatripgita/")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
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

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
