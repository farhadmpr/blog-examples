using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Blog
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
          Configuration.GetConnectionString("DefaultConnection")));
      services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>() // *** Added ***
        .AddEntityFrameworkStores<ApplicationDbContext>();
      services.AddControllersWithViews();
      services.AddRazorPages();
      
      services.AddSpaStaticFiles(spa =>
      {
        spa.RootPath = "wwwroot/admin";
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseSpaStaticFiles();
      app.UseMvcWithDefaultRoute();
      app.UseWhen((context) => (context.User.Identity.IsAuthenticated && context.User.IsInRole("Admin")), a =>
      {
          a.Map("/admin", adminApp =>
          {
              adminApp.UseSpa(spa => { });
          });
      });
    }
  }
}
