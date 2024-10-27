
using WebApiVersioning.Interface;
using WebApiVersioning.Mapping;
using WebApiVersioning.Repositories;

namespace WebApiVersioning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Repositories
            builder.Services.AddScoped<Icountry, CountryRepository>();
            builder.Services.AddAutoMapper(typeof(AutomapperProfiles));

            var app = builder.Build();

            // Configure the HTTP reque\\\\\peline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
