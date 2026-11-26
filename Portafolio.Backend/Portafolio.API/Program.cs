using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using Portafolio.API.Services;
using Portafolio.API.Data;
using Portafolio.API.Validators;
using Portafolio.API.Endpoints;
using Portafolio.API.Middlewares;
using Microsoft.OpenApi;



var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de CORS
// Busca esta parte y reemplázala:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // Leemos los orígenes permitidos desde una variable de entorno, 
        // o usamos "*" (cualquiera) si estamos en desarrollo local.
        var allowedOrigins = builder.Configuration["Cors:AllowedOrigins"]
            ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            ?? new[] { "*" };

        if (allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;

    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // 1. Definimos el esquema de seguridad (el candado)
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Ingresa el token JWT obtenido del login."
        };

        // 2. Aplicamos el requisito usando la nueva API de .NET 10
        // OpenApiSecurityRequirement ahora hereda de Dictionary<OpenApiSecuritySchemeReference, List<string>>
        var securityRequirement = new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        };

        // La seguridad a nivel de documento se aplica a todas las operaciones
        // En .NET 10 ya no existe document.SecurityRequirements, se usa el diccionario de Components o se aplica por operación.
        // Para el documento completo, lo correcto es usar el transformer para añadirlo a cada operación:
        if (document.Paths != null)
        {
            foreach (var path in document.Paths.Values)
            {
                // Agregamos la validación de que Operations no sea nulo
                if (path.Operations != null)
                {
                    foreach (var operation in path.Operations.Values)
                    {
                        operation.Security ??= new List<OpenApiSecurityRequirement>();
                        operation.Security.Add(securityRequirement);
                    }
                }
            }
        }

        return Task.CompletedTask;
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<ClienteCreateValidator>();
builder.Services.AddDbContext<PortafolioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"❌ ERROR DE AUTENTICACIÓN: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("✅ TOKEN VALIDADO CORRECTAMENTE");
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowFrontend");


app.MapOpenApi();
app.UseAuthentication();
app.UseAuthorization();

// 5. Mapeo de Endpoints (¡La magia de la limpieza!)
app.MapAuthEndpoints();
app.MapClientesEndpoints();
app.MapProductosEndpoints();
app.MapProveedoresEndpoints();
app.MapProductosProveedoresEndpoints();

app.Run();