using System.Text;
using GestaoColaboradores.API.Data;
using GestaoColaboradores.API.Repositories;
using GestaoColaboradores.API.Repositories.Interfaces;
using GestaoColaboradores.API.Services;
using GestaoColaboradores.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<IUnidadeRepository, UnidadeRepository>();

// Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IColaboradorService, ColaboradorService>();
builder.Services.AddScoped<IUnidadeService, UnidadeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ChaveSecretaParaDesenvolvimento2024!@#$";
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

// Controllers
builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gestão de Colaboradores API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Auto create database and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Garante que o schema existe (cria tabelas se não existirem)
    db.Database.EnsureCreated();

    // Seed: criar dados iniciais se não existirem
    if (!db.Usuarios.Any(u => u.Login == "admin"))
    {
        // Usuário admin
        var admin = new GestaoColaboradores.API.Models.Usuario
        {
            Codigo = "ADMIN001",
            Login = "admin",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Ativo = true
        };
        db.Usuarios.Add(admin);
        db.SaveChanges();

        // Unidade Matriz
        var matriz = new GestaoColaboradores.API.Models.Unidade
        {
            Codigo = "MATRIZ",
            Nome = "Matriz",
            Ativa = true
        };
        db.Unidades.Add(matriz);
        db.SaveChanges();

        // Colaborador Guilherme
        var colaborador = new GestaoColaboradores.API.Models.Colaborador
        {
            Codigo = "COL00001",
            Nome = "Guilherme Contratado Brito",
            UnidadeId = matriz.Id,
            UsuarioId = admin.Id
        };
        db.Colaboradores.Add(colaborador);
        db.SaveChanges();
    }
}

app.Run();
