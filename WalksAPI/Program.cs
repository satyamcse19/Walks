
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text;
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
            var startup = new Startup();
            startup.ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            startup.Configure(app, app.Environment);
            app.MapControllers();
            app.Run();
        }
    }
}
