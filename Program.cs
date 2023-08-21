using System.Text.Json; 
 
namespace ZooAPI 
{ 
    using ZooAPI.Controller; 
    using ZooAPI.Model; 
    using ZooAPI.Service; 
    using Microsoft.AspNetCore.Builder; 
    using Microsoft.AspNetCore.Http; 
    using Microsoft.Extensions.DependencyInjection; 
    using Microsoft.OpenApi.Models; 
 
    public class Program 
    { 
        public static void Main(string[] args) 
        { 
            var myAllowSpecificOrigins = "_myAllowSpecificOrigins"; 
            var builder = WebApplication.CreateBuilder(args); 
 
            // CORS aktivieren 
            builder.Services.AddCors(options => 
            { 
                options.AddPolicy(name: myAllowSpecificOrigins, 
                    policyBuilder => 
                    { 
                        policyBuilder.AllowAnyOrigin() 
                            .AllowAnyHeader() 
                            .AllowAnyMethod(); 
                    }); 
            }); 
 
            // Dienste konfigurieren 
            ConfigureServices(builder); 
 
            // Anwendung erstellen 
            var app = builder.Build(); 
 
            // CORS aktivieren 
            app.UseCors(myAllowSpecificOrigins); 
 
            // Anwendung konfigurieren 
            Configure(app); 
 
            app.Run(); 
        } 
 
        private static void ConfigureServices(WebApplicationBuilder builder) 
        { 
            // Dienste für API-Erkundung und Swagger hinzufügen 
            builder.Services.AddEndpointsApiExplorer(); 
            builder.Services.AddSwaggerGen(option => 
            { 
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Zoo API", Version = "v1" }); 
            }); 
 
            // Datenbankverbindung 
            string connectionString = builder.Configuration.GetConnectionString("ZooDb") ?? string.Empty; 
 
            // Dienste registrieren 
            builder.Services.AddScoped(_ => new DBConnection(connectionString, builder.Configuration)); 
            builder.Services.AddScoped<ZoobesucherService>(); 
            builder.Services.AddScoped<KassiererService>(); 
            builder.Services.AddScoped<TierpflegerService>(); 
 
            // Controller registrieren 
            builder.Services.AddControllers(); 
            builder.Services.AddScoped<ZoobesucherController>(); 
            builder.Services.AddScoped<KassiererController>(); 
            builder.Services.AddScoped<TierpflegerController>(); 
        } 
 
        private static void Configure(WebApplication app) 
        { 
            using var scope = app.Services.CreateScope(); 
 
            KassiererService kassiererService; 
            kassiererService = scope.ServiceProvider.GetRequiredService<KassiererService>(); 
 
            ZoobesucherService zoobesucherService; 
            zoobesucherService = scope.ServiceProvider.GetRequiredService<ZoobesucherService>(); 
 
            TierpflegerService tierpflegerService; 
            tierpflegerService = scope.ServiceProvider.GetRequiredService<TierpflegerService>(); 
 
            // Swagger einrichten 
            app.UseSwagger(); 
            app.UseSwaggerUI(option => 
            { 
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "zoo"); 
                option.RoutePrefix = string.Empty; 
            }); 
 
            // Kassierer-Endpunkte 
            app.MapControllers(); 
            app.MapControllerRoute("kassierer", "api/kassierer/{controller=Home}/{action=Index}/{id?}"); 
 
            // Tierpfleger-Endpunkte 
            app.MapControllerRoute("tierpfleger", "api/tierpfleger/{controller=Home}/{action=Index}/{id?}"); 
 
            // Zoobesucher-Endpunkte 
            app.MapControllerRoute("zoobesucher", "api/zoobesucher/{controller=Home}/{action=Index}/{id?}"); 
        } 
    } 
}