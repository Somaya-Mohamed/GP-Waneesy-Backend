using kidsApp.Application.Mapping;
using kidsApp.Application.ServiceManager;
using kidsApp.Application.Services.Classes;
using kidsApp.Application.Services.Interfaces;
using kidsApp.Domain.Contracts;
using kidsApp.Domain.Entities;
using kidsApp.Infrastructure.Data;
using kidsApp.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;

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

            // =========================
            // Identity (مع DbContext عادي)
            // =========================
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<KidsAppDbContext>()
                .AddDefaultTokenProviders();

            // =========================
            // JWT Authentication
            // =========================
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                    ),

                    // ⭐ مهم علشان [Authorize(Roles = "Admin")]
                    //RoleClaimType = "role"
                    RoleClaimType = ClaimTypes.Role
                };
            });

            // =========================
            // Authorization
            // =========================
            builder.Services.AddAuthorization();

            // ====================== AutoMapper ======================
            builder.Services.AddAutoMapper(cong => cong.AddProfile(new MappingProfile()), typeof(MappingProfile).Assembly);

            // ====================== Repositories & UnitOfWork ======================
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ====================== ServiceManager ======================
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<IAdminService, AdminService>();


            // ====================== Controllers & Swagger ======================
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "KidsApp API",
                    Version = "v1"
                });

                // ===== JWT Bearer Configuration =====
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
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
            app.UseAuthentication();   


            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}





