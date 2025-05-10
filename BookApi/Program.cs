//importing necessary namespaces
using System.Text;
using BookApi.Application.Interfaces;
using BookApi.Infrastructure.Data;
using BookApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using BookApi.Application.Features.Books.Commands;



//setup basic builder and dependencies
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// SWAGER or OpenAPI UI for testing in dev
builder.Services.AddOpenApi();

builder.Services.AddScoped<IBookRepository, BookRepository>();

// ✅ Register MediatR for CQRS handlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateBookHandler).Assembly));




//? configuring entity framework to use PostgreSQL using connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//? the JWT bit, using the secret key from appsettings.json
var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("JwtSettings:Secret is not configured in appsettings.json.");

//*also can do this 
/*
var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? Environment.GetEnvironmentVariable("JWT_SECRET")
    ?? throw new InvalidOperationException("JwtSettings:Secret is not configured.");
*/

//?adding the JWT-based authetication to the app
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

//*adding the controllers and authorization BEFORE the build
builder.Services.AddControllers();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200") // allow the Angular app
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


//like requiring express on node
var app = builder.Build();

//using swagger UI in dev (HTTP request pipeline)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//the root test route, like / in express
app.MapGet("/", () => "API is running...");

//*add map controllers before the app runs (routing http req to controller endpoints)
app.MapControllers();


//use the cords
app.UseCors();


//*enabling JWT and role policies in the app
app.UseAuthentication();
app.UseAuthorization();

//like app.listen in node to run the server
app.Run();

