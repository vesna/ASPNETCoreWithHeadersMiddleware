using Asp.Versioning;
using ASPNETCoreWithHeadersMiddleware.DTOs;
using ASPNETCoreWithHeadersMiddleware.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nelibur.ObjectMapper;

namespace ASPNETCoreWithHeadersMiddleware.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, string versionName,
            string versionNumber, string version = "v1")
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version,
                    new OpenApiInfo { Title = $"{versionName} {versionNumber}", Version = versionNumber });
            });
            return services;
        }

        public static IServiceCollection AddMappingConfig(this IServiceCollection services)
        {
            TinyMapper.Bind<Post, PostEntity>();
            TinyMapper.Bind<PostEntity, Post>();
            return services;
        }

        public static IServiceCollection AddVersioningConfig(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader()
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
    }
}
