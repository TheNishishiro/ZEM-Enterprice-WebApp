using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZEM_Enterprice_WebApp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using ZEM_Enterprice_WebApp.Requirements;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Serilog.Core;
using Microsoft.Extensions.Logging;

namespace ZEM_Enterprice_WebApp
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<MyUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddRazorPages().AddSessionStateTempDataProvider();
            services.AddSession();
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                config.MaxModelBindingCollectionSize = int.MaxValue;
            });

            services.Configure<FormOptions>(x => x.ValueCountLimit = int.MaxValue);

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy(DefaultRequirements.AdminOrTech.ToString(), policy => policy.Requirements.Add(new TechRequirement()));
                cfg.AddPolicy(DefaultRequirements.AdminOrOffice.ToString(), policy => policy.Requirements.Add(new OfficeRequirement()));
                cfg.AddPolicy(DefaultRequirements.AdminOrScanner.ToString(), policy => policy.Requirements.Add(new ScannerRequirement()));
                cfg.AddPolicy(DefaultRequirements.AdminOrMagazyn.ToString(), policy => policy.Requirements.Add(new MagRequirement()));
                cfg.AddPolicy(DefaultRequirements.AdminOrProd.ToString(), policy => policy.Requirements.Add(new ProdRequirement()));
                cfg.AddPolicy(DefaultRequirements.CanViewVT.ToString(), policy => policy.Requirements.Add(new CanViewVTRequirement()));
                cfg.AddPolicy(DefaultRequirements.CanViewSupply.ToString(), policy => policy.Requirements.Add(new CanViewSupplyRequirement()));
                cfg.AddPolicy(DefaultRequirements.CanViewVTScans.ToString(), policy => policy.Requirements.Add(new CanViewVTScansRequirement()));
                cfg.AddPolicy(DefaultRequirements.CanDoAnal.ToString(), policy => policy.Requirements.Add(new CanDoAnalRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, TechReqHandler>();
            services.AddSingleton<IAuthorizationHandler, OfficeReqHandler>();
            services.AddSingleton<IAuthorizationHandler, ScannerReqHandler>();
            services.AddSingleton<IAuthorizationHandler, MagReqHandler>();
            services.AddSingleton<IAuthorizationHandler, ProdReqHandler>();
            services.AddSingleton<IAuthorizationHandler, CanViewVTReqHandler>();
            services.AddSingleton<IAuthorizationHandler, CanViewSupplyReqHandler>();
            services.AddSingleton<IAuthorizationHandler, CanViewVTScansReqHandler>();
            services.AddSingleton<IAuthorizationHandler, CanDoAnalReqHandler>();

            services.ConfigureApplicationCookie(opt => opt.LoginPath = "/Login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            CreateRoles.Default(serviceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
