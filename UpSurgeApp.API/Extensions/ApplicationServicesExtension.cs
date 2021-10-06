using Core.Interface;
using Infrastructure.Data.Repository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using UpSurgeApp.API.Errors;

namespace UpSurgeApp.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountRepository, AccountService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericService<>));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddScoped<IForecastService, ForecastService>();
            //services.AddScoped<IMatchmakerRepository, MatchmakerRepository>();
            services.AddScoped<IMatchmakerService, MatchmakerService>();
            services.AddScoped<ISymbolService, SymbolService>();
            services.AddScoped<ISentimentsService, SentimentsService>();
            services.AddScoped<IAnalystRatingService, AnalystRatingService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

    }
}
