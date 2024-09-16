using GranjaLosAres_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GranjaLosAres_API.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.

// Agregar la configuraci�n de la cadena de conexi�n de la base de datos desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("GranjaAres1Database");

// Configuraci�n del DbContext con SQL Server
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Inyecci�n de dependencias para los servicios
builder.Services.AddScoped<IClasificacionHuevoService, ClasificacionHuevoService>();
builder.Services.AddScoped<ICorralService, CorralService>();
builder.Services.AddScoped<IEstadoLoteService, EstadoLoteService>();
builder.Services.AddScoped<IProduccionService, ProduccionService>();
builder.Services.AddScoped<IRazaGService, RazaGService>();
builder.Services.AddScoped<ILoteService, LoteService>();
builder.Services.AddScoped<IVentasService, VentasService>();

// Habilitar CORS (configura los or�genes que necesites permitir)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Token inv�lido: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token v�lido para: " + context.Principal.Identity.Name);
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



builder.Services.AddAuthorization();


// Agregar los controladores
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Definici�n de seguridad para JWT
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

// Redirecci�n HTTP a HTTPS
app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAllOrigins");

// Habilitar autenticaci�n y autorizaci�n
app.UseAuthentication();  // Necesario para la autenticaci�n
app.UseAuthorization();   // Necesario para la autorizaci�n

// Mapear los controladores
app.MapControllers();

// Ejecutar la aplicaci�n
app.Run();
