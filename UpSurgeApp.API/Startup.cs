using AutoMapper;
using Infrastructure.Cron;
using Infrastructure.Data;
using Infrastructure.SentiOne;
using Infrastructure.Sygnal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Polly;
using Polly.Extensions.Http;
using Quotemedia;
using System;
using System.IO;
using System.Net.Http;
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
            //services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();

            services.AddAutoMapper(typeof(MappingProfiles));          

            /* It is gonna live for the lifetime of the REQUEST */

            services.AddDbContext<UpSurgeAppDbContext>(options =>
                   options.UseSqlServer(
                       Configuration.GetConnectionString("UpSurgeAppConnection"), 
                       assembly => assembly.MigrationsAssembly("Infrastructure")
                       )
                   );

            services
                .AddHttpClient<QuotemediaHttpClient>(configClient =>
                {
                    configClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Configuration["Quotemedia:Token"]);
                    configClient.BaseAddress = new Uri(Configuration["Quotemedia:BaseAddress"]);
                });

            services
                .AddHttpClient<SentiOneHttpClient>(configClient =>
                {
                    configClient.DefaultRequestHeaders.Add("X-API-KEY", Configuration["Sentione:X-API-KEY"]);
                    configClient.BaseAddress = new Uri(Configuration["Sentione:BaseAddress"]);
                });

            var sygnalBaseAddress = Configuration["Sygnal:BaseAddress"];
            var sygnalUser = Configuration["Sygnal:User"];
            var sygnalPassword = Configuration["Sygnal:Password"];
            var authenticationString = $"{sygnalUser}:{sygnalPassword}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
            services
                .AddHttpClient<SygnalHttpClient>(configClient => {                                        
                    configClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                    configClient.BaseAddress = new Uri(sygnalBaseAddress);
                    configClient.Timeout = TimeSpan.FromMinutes(10);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(20))
                .AddPolicyHandler(GetSygnalHttpClientRetryPolicy());

            /* Adding Services Extensions */
            services.AddApplicationServices();
            services.AddSwaggerDocumentation();
            services.AddIdentityServices(Configuration);

            // Crone
            //services.AddCronJob<ForecastCronJob>(c =>
            //{
            //    c.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            //    c.CronExpression = @"20 8 * * *";
            //});
        }

        static IAsyncPolicy<HttpResponseMessage> GetSygnalHttpClientRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
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
