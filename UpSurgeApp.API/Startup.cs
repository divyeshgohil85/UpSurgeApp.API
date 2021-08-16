using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using UpSurgeApp.API.Extensions;
using UpSurgeApp.API.Helpers;

namespace UpSurgeApp.API
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

            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();
            services.AddAutoMapper(typeof(MappingProfiles));

          

            /* It is gonna live for the lifetime of the REQUEST */

            services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("UpSurgeAppConnection"),
                   assembly => assembly.MigrationsAssembly("Infrastructure")));

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"),
            assembly => assembly.MigrationsAssembly("Infrastructure")));


            /* Adding Services Extensions */
            services.AddApplicationServices();
            services.AddSwaggerDocumentation();
            services.AddIdentityServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            /* Need to remove the IF condition to make it working on the production*/
            app.UseDeveloperExceptionPage();
            app.UseSwaggerDocumentation();
          //  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UpSurgeApp.API v1"));
            // }







            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, "wwwroot/")),
                RequestPath = "/wwwroot"
            });
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
