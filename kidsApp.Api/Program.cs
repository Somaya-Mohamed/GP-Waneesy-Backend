using kidsApp.Application.Mapping;
using kidsApp.Application.ServiceManager;
using kidsApp.Domain.Contracts;
using kidsApp.Infrastructure.Data;
using kidsApp.Infrastructure.unitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;

namespace kidsApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ====================== Database ======================
            builder.Services.AddDbContext<KidsAppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")
                                     ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

            // ====================== AutoMapper ======================
            builder.Services.AddAutoMapper(cong => cong.AddProfile(new MappingProfile()), typeof(MappingProfile).Assembly);

            // ====================== Repositories & UnitOfWork ======================
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ====================== ServiceManager ======================
            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            // ====================== Controllers & Swagger ======================
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "KidsApp API", Version = "v1" });
                //  JWT :
                // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { ... });
            });

            // ====================== CORS (Frontend) ======================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // ====================== Middleware Pipeline ======================
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                    //(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KidsApp API v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll"); 

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}





//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<KidsAppDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();


//app.MapControllers();

//app.Run();

