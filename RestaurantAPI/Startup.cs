using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Services;
using RestaurantAPI.Middleware;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantAPI.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;

namespace RestaurantAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();
            

            Configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddSingleton(authenticationSettings);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            }); //token
            services.AddControllers().AddFluentValidation();
            
            services.AddAuthorization(option =>
            {
                option.AddPolicy("HasNationality",builder => 
                builder.RequireClaim("Nationality")); // po Nationality mo¿na podaæ konkretna wartosc claimu np. "German, TwóStary"

                option.AddPolicy("Atleast20", builder => 
                builder.AddRequirements(new MinimumAgeRequirment(20)));

                option.AddPolicy("CreatedAtleast2Restaurants",
                    builder =>
                    builder.AddRequirements(new CreatedMultipleRestaurantsRequirment(2)));


            }); // dodawanie w³aœnej polityki o nazwie HasNationality.

            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirmentHandler>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequiremtHandler>(); 
            services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsRequirmentHandler>(); 

            services.AddDbContext<RestaurantDbContext>(option=> option.UseSqlServer(Configuration.GetConnectionString("RestaurantDbConnection")));
            services.AddScoped<RestaurantSeeder>();

            services.AddAutoMapper(this.GetType().Assembly);

            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();

            services.AddScoped<IUserContextService, UserContextService>();
            services.AddHttpContextAccessor();

            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", builder =>
                {
                    builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();// WithOrigin(Configuration["AllowedOrigins"]) dla konkretnej domeny przez appsettings.json
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,RestaurantSeeder seeder)
        {
            
            app.UseResponseCaching();
            app.UseStaticFiles();
            seeder.Seed(); // seeduje za kazdym razem z jakiegos powodu 

            app.UseCors("FrontEndClient");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>(); // middleware do self exceptionów
            app.UseMiddleware<RequestTimeMiddleware>();// koniecznie przed w httpred otherwise expection wyskoczy przez zapytaniem!

            app.UseAuthentication(); // tez przed
            app.UseHttpsRedirection(); //wa¿ne

            app.UseSwagger(); //endpointy w swaggerze
            app.UseSwaggerUI(c=>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json","RestaurantAPI");
            });

            app.UseRouting();
           
            app.UseAuthorization(); //koniecznie w tym miejscu 

           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
   
}
