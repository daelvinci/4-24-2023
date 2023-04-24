using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Services;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<PustokDbContext>(opt =>
            opt.UseSqlServer("Server=DESKTOP-Q1QK6V2\\SQLEXPRESS; Database=PustokDb; Trusted_Connection=true")
            );
            builder.Services.AddScoped<LayoutService>();
            //builder.Services.AddSession(opt=>
            //{
            //    opt.IdleTimeout = TimeSpan.FromMinutes(20);
            //});

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseSession();


            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
          
            app.Run();
        }
    }
}