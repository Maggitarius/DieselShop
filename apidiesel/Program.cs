using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.EntityFrameworkCore;
using apidiesel.Models;

namespace apidiesel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = false;
            });

            builder.Services.AddDbContext<dieselContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("con"))
            );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllWithCredentials", policy =>
                {
                    policy.SetIsOriginAllowed(origin => true)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(7132, listenOptions =>
                {
                    listenOptions.UseHttps("cert.pfx", "1234");
                });
            });

            var app = builder.Build();

            app.UseCors("AllowAllWithCredentials");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
