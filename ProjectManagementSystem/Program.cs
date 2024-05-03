using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Services;
using System.Configuration;

namespace ProjectManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                                options.SlidingExpiration = true;
                                options.AccessDeniedPath = "/Forbidden/";
                                options.LoginPath = "/Index";
                                options.Cookie.IsEssential = true;
                            });

            builder.Services.AddDbContext<PMSContext>(
                options => options
                                .UseSqlServer(builder.Configuration.GetConnectionString("SqlConn"))
            );

            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("AppSettings:SMTP"));
            builder.Services.AddSingleton<EmailService>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}