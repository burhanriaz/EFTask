
using EFTask.Data;
using EFTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;


namespace EFTask
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
            services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            {
                Options.Password.RequiredLength = 6;
                Options.Password.RequireNonAlphanumeric = false;
                Options.Password.RequiredUniqueChars = 3;

            }).AddEntityFrameworkStores<ApplicationDbContext>();
            // Same work the below line of code
            //services.Configure<IdentityOptions>(Options =>
            //{
            //    Options.Password.RequiredLength = 6;
            //    Options.Password.RequireNonAlphanumeric = false;
            //    Options.Password.RequiredUniqueChars = 3;
            //});

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // Appling  Authorize attribute on whole application globally
            services.AddMvc(Options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
                Options.Filters.Add(new AuthorizeFilter(policy));
            });

            //The first parameter is the name of the policy and the second parameter is the policy itself
            services.AddAuthorization(options =>
            {
                //Claim Policy
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));


                //Edit Role Policy
                options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role"));

                //role policy
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));

                //we can alsoadd multli role in policy 
                // options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin","User","Employee"));
            });
            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectin")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
