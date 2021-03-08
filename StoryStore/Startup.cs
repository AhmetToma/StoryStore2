using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoryStore.Data;
using StoryStore.DataModels;
using StoryStore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoryStore
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

            services.AddDbContext<StoryStoreDbContext>(builder =>
            { builder.UseSqlServer(Configuration.GetConnectionString("Default")); });
       
            services.AddControllersWithViews();
            services.AddMvc().AddRazorRuntimeCompilation();
            services.AddIdentity<AppUser, Microsoft.AspNetCore.Identity.IdentityRole>()
           .AddEntityFrameworkStores<StoryStoreDbContext>().AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(25);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = false;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(25);
                options.LoginPath = "/Home/Login";
                //  options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddHttpContextAccessor();

        }

        private async Task CreateDefaultAdmin(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var db = serviceProvider.GetRequiredService<StoryStoreDbContext>();
            var ageRanges = await db.AgeRanges.ToListAsync();
            if(ageRanges.Count==0)
            {
                List<AgeRange> newAgeRanges = new List<AgeRange>()
                {
                    new AgeRange(){Range="3-7"},
                    new AgeRange(){Range="4-10"},
                    new AgeRange(){Range="1-15"},
                    new AgeRange(){Range="16-20"},
                };
                db.AgeRanges.AddRange(newAgeRanges);
                db.SaveChanges();
            }


            string[] roleNames = { "User" ,"Admin"};
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var user = await userManager.FindByEmailAsync("Admin@gmail.com");

            if (user == null)
            {
             
                var poweruser = new AppUser
                {
                    UserName = "Admin@gmail.com",
                    Email = "Admin@gmail.com",
                    AgeRangeId=2
                };
                var adminPassword = "aAj120_*!";

                var createPowerUser = await userManager.CreateAsync(poweruser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await userManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }


        

     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Dashboard}/{id?}");
            });

            CreateDefaultAdmin(serviceProvider).GetAwaiter().GetResult();
       
        }
    }
}
