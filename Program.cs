using MainPage.Repositories;
using MySql.Data.MySqlClient;
using Skills.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5260");

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped(sp =>{
    var con = new MySqlConnection(builder.Configuration["ConnectionStrings:MySql"]);
    con.Open();
    return con;

});

builder.Services.AddScoped<UserRepository.Repositories.UserRepository>();
builder.Services.AddScoped<MainpageRepository>();
builder.Services.AddScoped<SkillsRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();


app.MapControllers();

app.Run();
