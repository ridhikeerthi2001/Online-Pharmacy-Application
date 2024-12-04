using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using Microsoft.EntityFrameworkCore;
using Pharmacy_DAO;

namespace Online_Pharmacy_App_MVC
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();
            builder.Services.AddDNTCaptcha(options => options.UseCookieStorageProvider().ShowThousandsSeparators(false));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
