using GranjaLosAres_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GranjaLosAres_API.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.

// Agregar la configuración de la cadena de conexión de la base de datos desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("GranjaAres1Database");

// Configuración del DbContext con SQL Server
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Inyección de dependencias para los servicios
builder.Services.AddScoped<IClasificacionHuevoService, ClasificacionHuevoService>();
builder.Services.AddScoped<ICorralService, CorralService>();
builder.Services.AddScoped<IEstadoLoteService, EstadoLoteService>();
builder.Services.AddScoped<IProduccionService, ProduccionService>();
builder.Services.AddScoped<IRazaGService, RazaGService>();
builder.Services.AddScoped<ILoteService, LoteService>();
builder.Services.AddScoped<IVentasService, VentasService>();

// Habilitar CORS (configura los orígenes que necesites permitir)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Habilitar autenticación y JWT
var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtKey))
{
    throw new ArgumentNullException("Jwt:Key", "La clave JWT no está configurada o es nula.");
}

Console.WriteLine($"JWT Key: {jwtKey}"); // Para depuración

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

// Agregar los controladores
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Definición de seguridad para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            new string[] {}
        }
    });
});

// 2. Build the app
var app = builder.Build();

// 3. Configure the HTTP request pipeline.

// Solo habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// Redirección HTTP a HTTPS
app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAllOrigins");

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// Ejecutar la aplicación
app.Run();
