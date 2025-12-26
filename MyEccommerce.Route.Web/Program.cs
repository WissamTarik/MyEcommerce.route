
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Entities.Identity;
using MyEccommerce.Route.Persistence;
using MyEccommerce.Route.Persistence.Identity;
using MyEccommerce.Route.Persistence.Notifications.Hubs;
using MyEccommerce.Route.Persistence.Repositories;
using MyEccommerce.Route.Presentation;
using MyEccommerce.Route.Services;
using MyEccommerce.Route.Services.Abstractions;
using MyEccommerce.Route.Services.Abstractions.Emails;
using MyEccommerce.Route.Services.Abstractions.Notifications;
using MyEccommerce.Route.Services.Emails;
using MyEccommerce.Route.Services.Mapping.Auth;
using MyEccommerce.Route.Services.Mapping.Baskets;
using MyEccommerce.Route.Services.Mapping.Orders;
using MyEccommerce.Route.Services.Mapping.Products;
using MyEccommerce.Route.Services.Notifications;
using MyEccommerce.Route.Shared.Email;
using MyEccommerce.Route.Shared.Jwt;
using MyEccommerce.Route.Shared.StripeData;
using MyEccommerce.Route.Web.HandleErrors;
using MyEccommerce.Route.Web.Helpers;
using MyEcommerce.Route.Persistence.Contexts;
using StackExchange.Redis;
using Stripe.Terminal;
using System.Text;
using System.Threading.Tasks;
namespace MyEccommerce.Route.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          
            // Add services to the container.

            builder.Services.AddControllers()
                    .AddApplicationPart(typeof(PresentationAssemblyMarker).Assembly);

            //  .AddApplicationPart(typeof(MyEccommerce.Route.Presentation.Controllers.ProductsController).Assembly);
            ;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyEcommerce API", Version = "v1" });

                var webXml = Path.Combine(AppContext.BaseDirectory, "MyEccommerce.Route.Web.xml");
                if (File.Exists(webXml))
                    c.IncludeXmlComments(webXml, true);

                var presentationXml = Path.Combine(AppContext.BaseDirectory, "MyEccommerce.Route.Presentation.xml");
                if (File.Exists(presentationXml))
                    c.IncludeXmlComments(presentationXml, true);

                var sharedXml = Path.Combine(AppContext.BaseDirectory, "MyEccommerce.Route.Shared.xml");
                c.IncludeXmlComments(sharedXml, true);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name="Authorization",
                    Type=SecuritySchemeType.Http,
                    Scheme="Bearer",
                    In =ParameterLocation.Header,
                    Description="Please enter a JWT token like: Bearer {your token}"
                });
              
                c.CustomSchemaIds(type => type.FullName);
            });
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IUnitOfWok, UnitOfWork>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<IEmailService , EmailService>();
            builder.Services.AddScoped<IImageService , ImageService>();
            builder.Services.AddScoped<INotificationService , NotificationService>();
            builder.Services.AddSingleton<IUserIdProvider, EmailUserIdProvider>();
            builder.Services.AddAutoMapper((config) =>
            {
                config.AddProfile(new ProductProfile(builder.Configuration));
                config.AddProfile(new BasketProfile());
                config.AddProfile(new UserProfile());
                config.AddProfile(new OrderProfile());
            });
            builder.Services.AddDbContext<MyEcommerceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<MyEcommerceIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            }).AddIdentity<AppUser,IdentityRole>().
             AddEntityFrameworkStores<MyEcommerceIdentityDbContext>()
             .AddDefaultTokenProviders()
             ;
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(builder.Configuration["Redis"])
                );
            builder.Services.Configure<ApiBehaviorOptions>((config) =>
            {
                config.InvalidModelStateResponseFactory = (apiContext) =>
                {
                    var error = apiContext.ModelState.Where((m) => m.Value.Errors.Any())
                                                   .Select((m) => new ValidationError()
                                                   {
                                                       Field = m.Key,
                                                       Message = m.Value.Errors.Select((e) => e.ErrorMessage)
                                                   });
                    var Response = new ValidationErrorResponse() { Errors = error };
                    return new BadRequestObjectResult(Response);
                };


            });
            builder.Services.Configure<EmailSettingsOptions>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))

                };


                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var access_token = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if(!string.IsNullOrEmpty(access_token)
                            && path.StartsWithSegments("/hubs/notifications"))
                        {
                            context.Token = access_token;
                        }
                        return Task.CompletedTask;
                    }
                };



            });

            builder.Services.Configure<StripeOption>(builder.Configuration.GetSection("StripeOptions"));
            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name:"AllowAll",policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod()
                    .WithOrigins("http://127.0.0.1:5500", "https://localhost:5500")
                    .AllowCredentials();
                });
            });
            var app = builder.Build();

           using  var Scope= app.Services.CreateScope();
            var DbInitializer = Scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await  DbInitializer.InitializeDbAsync();
            await DbInitializer.InitializeIdentityDbAsync();




            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c=>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyEcommerce v1");
                    c.RoutePrefix = "swagger";
                });
            }
            app.UseStaticFiles();
            app.UseMiddleware<GlobalErrorHandling>();
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<NotificationHub>("/hubs/notifications");
            app.Run();
        }
    }
}
