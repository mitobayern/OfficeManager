namespace OfficeManager
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using OfficeManager.Data;
    using OfficeManager.SeedData;
    using OfficeManager.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                });

            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddTransient<ITenantsService, TenantsService>();
            services.AddTransient<IOfficesService, OfficesService>();
            services.AddTransient<ILandlordsService, LandlordsService>();
            services.AddTransient<IElectricityMetersService, ElectricityMetersService>();
            services.AddTransient<ITemperatureMetersService, TemperatureMetersService>();
            services.AddTransient<IMeasurementsService, MeasurementsService>();
            services.AddTransient<IPricesInformationService, PricesInformationService>();
            services.AddTransient<IAccountingReportsService, AccountingReportsService>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IHtmlToPdfConverter, HtmlToPdfConverter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scopedService = app.ApplicationServices.CreateScope())
            {
                var dbContext = scopedService.ServiceProvider.GetService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder(dbContext, scopedService.ServiceProvider)
                    .SeedDataAsync()
                    .GetAwaiter()
                    .GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "administrationRoute",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
