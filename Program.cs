using MySql.Data.MySqlClient;
using Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
AddJwt(builder);
builder.WebHost.UseUrls("http://localhost:5260");

builder.Services.AddControllers();

builder.Services.AddAuthorization();



builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped(sp =>
{
    var con = new MySqlConnection(builder.Configuration["ConnectionStrings:MySql"]);
    con.Open();
    return con;
});

builder.Services.AddScoped<Repositories.MainpageRepository>();
builder.Services.AddScoped<Repositories.SkillsRepository>();
builder.Services.AddScoped<Repositories.UserRepository>();
builder.Services.AddScoped<Repositories.ProjectsRepository>();
builder.Services.AddScoped<Repositories.RoleRepository>();
builder.Services.AddScoped<Repositories.EducationRepository>();
builder.Services.AddScoped<Repositories.BlogRepository>();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


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