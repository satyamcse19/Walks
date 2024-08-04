
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WalksAPI.Data;
using WalksAPI.Interfaces.Repositories;
using WalksAPI.Mapping;
using WalksAPI.Repositories;

namespace WalksAPI
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

            builder.Services.AddDbContext<WalkDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("WalksConnectionStrings")));
            builder.Services.AddScoped<IRegionRepository,SQLRegionRepository>();
            builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
            builder.Services.AddAutoMapper(typeof(AutomapperProfiles));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
