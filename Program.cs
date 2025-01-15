using MySql.Data.MySqlClient;
using Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5260");

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// CORS yapılandırması ekleniyor
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
builder.Services.AddScoped<Repositories.BlogRepository>();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
