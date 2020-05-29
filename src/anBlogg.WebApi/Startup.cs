using anBlogg.Application;
using anBlogg.Infrastructure;
using anBlogg.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;

namespace anBlogg.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public static readonly ILoggerFactory Logger = CreateLogger();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlogContext>(SetupDbContext);

            void SetupDbContext(DbContextOptionsBuilder builder)
            {
                builder.UseSqlServer(Configuration["Data:AppDb:ConnectionString"]);
                builder.UseLoggerFactory(Logger);
                builder.EnableSensitiveDataLogging();
            }

            services.AddControllers(SetupControllers)
                .AddNewtonsoftJson(SetupNewtonsoftJson);

            static void SetupControllers(MvcOptions options) =>
                options.ReturnHttpNotAcceptable = true;

            static void SetupNewtonsoftJson(MvcNewtonsoftJsonOptions options) =>
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();

            services.AddCors(SetupCors);

            static void SetupCors(CorsOptions options) =>
                options.AddPolicy("MyPolicy", SetupPolicyBuilder);

            static void SetupPolicyBuilder(CorsPolicyBuilder builder) =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

            services.AddDomain();

            services.AddApplication();

            services.AddInfrastructure();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.UseEndpoints(SetupEndpoints);

            static void SetupEndpoints(IEndpointRouteBuilder builder) =>
                builder.MapControllers(); ;
        }

        private static ILoggerFactory CreateLogger()
        {
            return LoggerFactory.Create(SetupLoggerFactory);

            static void SetupLoggerFactory(ILoggingBuilder builder) =>
                builder.AddConsole();
        }
    }
}