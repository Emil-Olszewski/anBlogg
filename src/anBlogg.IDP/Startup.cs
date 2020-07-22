// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using anBlogg.IDP.Data;
using anBlogg.IDP.Models;
using IdentityServer4.Configuration;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace anBlogg.IDP
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = GetMigrationAssemblyName();

            services.AddControllersWithViews();

            services.AddDbContext<anBloggIDPContext>(SetupDbContext);

            void SetupDbContext(DbContextOptionsBuilder options)
            {
                options.UseSqlServer(Configuration["Data:AppDb:ConnectionString"],
                sql => sql.MigrationsAssembly(migrationsAssembly));
            }

            services.AddDbContext<Data.ConfigurationDbContext>(SetupConfigurationDbContext);

            void SetupConfigurationDbContext(DbContextOptionsBuilder options)
            {
                options.UseSqlServer(Configuration["Data:AppDb:ConnectionString"],
                sql => sql.MigrationsAssembly(migrationsAssembly));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>(SetupIdentity)
                .AddEntityFrameworkStores<anBloggIDPContext>()
                .AddDefaultTokenProviders();

            static void SetupIdentity(IdentityOptions options)
            {
                options.SignIn.RequireConfirmedEmail = false;
            }

            var builder = services.AddIdentityServer(SetupIdentityServer)
                .AddConfigurationStore(SetupConfigurationStore)
                .AddOperationalStore(SetupOperationalStore)
                .AddAspNetIdentity<ApplicationUser>();

            static void SetupIdentityServer(IdentityServerOptions options)
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.UserInteraction.LoginUrl = "/Account/Login";
                options.UserInteraction.LogoutUrl = "/Account/Logout";
                options.Authentication = new AuthenticationOptions()
                {
                    CookieLifetime = TimeSpan.FromHours(10),
                    CookieSlidingExpiration = true
                };
            }

            void SetupConfigurationStore(ConfigurationStoreOptions options)
            {
                options.ConfigureDbContext =
                    b => b.UseSqlServer(Configuration["Data:AppDb:ConnectionString"],
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            }

            void SetupOperationalStore(OperationalStoreOptions options)
            {
                options.ConfigureDbContext =
                    b => b.UseSqlServer(Configuration["Data:AppDb:ConnectionString"],
                    sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
            }

            var loggerFactory = (ILoggerFactory)new LoggerFactory();
            var cors = new DefaultCorsPolicyService(loggerFactory.CreateLogger<DefaultCorsPolicyService>())
            {
                AllowAll = true
            };

            services.AddSingleton<ICorsPolicyService>(cors);

            if (Environment.IsDevelopment())
                builder.AddDeveloperSigningCredential();
            else
                throw new Exception("need to configure key material");
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }

        private string GetMigrationAssemblyName()
        {
            var typeofStartup = typeof(Startup);
            var typeInfo = typeofStartup.GetTypeInfo();
            var assemblyName = typeInfo.Assembly.GetName();
            return assemblyName.Name;
        }
    }
}