using AutoMapper;
using anBlogg.Application;
using anBlogg.Infrastructure;
using anBlogg.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace anBlogg.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public static readonly ILoggerFactory Logger
            = LoggerFactory.Create(builder => 
            { 
                builder.AddConsole(); 
            });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlogContext>(options =>
            {
                options.UseSqlServer(Configuration["Data:AppDb:ConnectionString"]);
                options.UseLoggerFactory(Logger);
                options.EnableSensitiveDataLogging();
            });

            services
            .AddControllers(options => 
            {
                options.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson(options => 
            {
                options.SerializerSettings.ContractResolver = 
                    new CamelCasePropertyNamesContractResolver();
            });

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
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
