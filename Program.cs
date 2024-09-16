using GranjaLosAres_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.

// Agregar la configuraci�n de la cadena de conexi�n de la base de datos desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("GranjaAres1Database");

// Configuraci�n del DbContext con SQL Server
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Inyecci�n de dependencias para los servicios (como IClienteService)
//builder.Services.AddScoped<IClienteService, ClienteService>();

// Habilitar CORS (configura los or�genes que necesites permitir)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Habilitar autenticaci�n y JWT (si es necesario)
var jwtKey = builder.Configuration["Jwt:Key"];
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

// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configuraci�n de JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingrese el token JWT en este formato: Bearer {token}",
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

// Redirecci�n HTTP a HTTPS
app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAllOrigins");

// Habilitar autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// Ejecutar la aplicaci�n
app.Run();
