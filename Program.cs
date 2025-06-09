using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication Ayarı
AddJwt(builder);

// CORS Ayarı
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// MySQL bağlantısı (Dapper için açık bağlantı)
builder.Services.AddScoped(sp =>
{
    var con = new MySqlConnection(builder.Configuration["ConnectionStrings:DefaultConnection"]);
    con.Open();
    return con;
});

// Repository’ler
builder.Services.AddScoped<MainpageRepository>();
builder.Services.AddScoped<SkillsRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProjectsRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<EducationRepository>();
builder.Services.AddScoped<BlogRepository>();

// Controller desteği
builder.Services.AddControllers();

// Swagger Desteği
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token'ınızı buraya girin (Bearer olmadan)"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// Authorization
builder.Services.AddAuthorization();

builder.WebHost.UseUrls("http://localhost:5260");

var app = builder.Build();

// Middleware’ler
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.MapControllers();

app.Run();

// JWT metodu
static void AddJwt(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });
}
