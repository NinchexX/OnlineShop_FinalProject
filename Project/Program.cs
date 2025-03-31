
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Project.Configuration.Middlewares;
using Project.DB;
using Project.Entities;
using Project.Interfaces;
using Project.Services;
using System.Data;
using System.Text;

namespace Project
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration;

            builder.Services.AddControllers();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineShop" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Bearer token authentication. Enter 'Bearer' [space] and then your token in the text input below.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddDbContext<ProjectDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IEmailSender, EmailSender>();

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<ProjectDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(configuration["SecretKey"])),
                   ClockSkew = TimeSpan.FromMinutes(5)
               };
           });

            
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IOrderingService, OrderingService>();


            builder.Services.AddAuthorization();

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (IServiceScope scope = app.Services.CreateScope())
            {
                ProjectDbContext context = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
                if (context != null)
                {
                    await DBInitializer.InitializeDatabase(app.Services, context);
                }
            }

            app.MapControllers();

            app.UseMiddleware<TransactionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication()
               .UseAuthorization();

            app.Run();
        }
    }
}
