using ASPNETCoreWithHeadersMiddleware.Configuration;
using ASPNETCoreWithHeadersMiddleware.Entities;
using ASPNETCoreWithHeadersMiddleware.Extensions;
using ASPNETCoreWithHeadersMiddleware.Filters;
using ASPNETCoreWithHeadersMiddleware.Handlers;
using ASPNETCoreWithHeadersMiddleware.Handlers.Interfaces;
using ASPNETCoreWithHeadersMiddleware.Middleware;
using ASPNETCoreWithHeadersMiddleware.Services;
using ASPNETCoreWithHeadersMiddleware.Services.Interfaces;

namespace ASPNETCoreWithHeadersMiddleware
{
    public class Startup
    {
        private const string VersionName = "ASPNETCoreWithHeadersMiddleware";
        private const string VersionNumber = "v1.0";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddVersioningConfig();
            services.AddControllers(options =>
            {
                options.MaxModelValidationErrors = 100;
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;

            })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                    options.SuppressModelStateInvalidFilter = true;
                });

            services.AddSwaggerConfig(VersionName, VersionNumber);
            services.Configure<HeadersMiddlewareSettings>(Configuration.GetSection("HeadersMiddleware"));
            services.AddSingleton<IRedisHandler<PostEntity, string>>(sp => new RedisPostHandler(Configuration["Redis:Connection"]));
            services.AddScoped<IPostService, PostService>();
            services.AddMappingConfig();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<HeadersMiddleware>();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
