using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkyAPI.Data;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ParkyAPI.ParkyMapper;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkyAPI
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //add for access NationalParkRepository methos from any controller repsitory pattern
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();

            //add auto mapper
            services.AddAutoMapper(typeof(ParkyMappings));

            //add api version control
            services.AddApiVersioning(options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                //add version of api below the api response 
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            //This Code replace by below code for get dynamic configure from ConfigureSwaggerOptions.cs
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            //add swagger
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("ParkyOpenAPISpec",
            //        new Microsoft.OpenApi.Models.OpenApiInfo()
            //        {
            //            Title = "ParkyAPI",
            //            Version = "1",
            //            Description = "Udemy Parky",
            //            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            //            {
            //                Email = "Ebrahimi.mr@gmail.com",
            //                Name = "MohammadReza Ebrahimi",
            //                Url = new Uri("http://www.MreOnline.com"),
            //            },
            //            License = new Microsoft.OpenApi.Models.OpenApiLicense
            //            {
            //                Name = "MIT License",
            //                Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //            }

            //        });
                //options.SwaggerDoc("ParkyOpenAPISpecNP",
                //    new Microsoft.OpenApi.Models.OpenApiInfo()
                //    {
                //        Title = "ParkyAPI (NationalPark)",
                //        Version = "1",
                //        Description = "Udemy Parky API NationalPark",
                //        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                //        {
                //            Email = "Ebrahimi.mr@gmail.com",
                //            Name = "MohammadReza Ebrahimi",
                //            Url = new Uri("http://www.MreOnline.com"),
                //        },
                //        License = new Microsoft.OpenApi.Models.OpenApiLicense
                //        {
                //            Name = "MIT License",
                //            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                //        }

                //    });
                //options.SwaggerDoc("ParkyOpenAPISpecTrails",
                //   new Microsoft.OpenApi.Models.OpenApiInfo()
                //   {
                //       Title = "ParkyAPI (Trails)",
                //       Version = "1",
                //       Description = "Udemy Parky API Trails",
                //       Contact = new Microsoft.OpenApi.Models.OpenApiContact
                //       {
                //           Email = "Ebrahimi.mr@gmail.com",
                //           Name = "MohammadReza Ebrahimi",
                //           Url = new Uri("http://www.MreOnline.com"),
                //       },
                //       License = new Microsoft.OpenApi.Models.OpenApiLicense
                //       {
                //           Name = "MIT License",
                //           Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                //       }

                //   });
                //add this code for automatic load document from xml in swagger
            //    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //    options.IncludeXmlComments(xmlCommentFullPath);

            //});
            services.AddControllers();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // add swagger and use this url fo see json value
            //https://localhost:44346/swagger/ParkyOpenAPISpec/swagger.json
            app.UseSwagger();

            // add for UI View

            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant ());
                    options.RoutePrefix = "";
                }
            });

            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
            //    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecNP/swagger.json", "Parky API NP");
            //    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");
            //    //add for set swagger open as default 
            //    options.RoutePrefix = "";
            //});
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
