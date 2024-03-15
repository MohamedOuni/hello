using EY.Energy.Application.EmailConfiguration;
using EY.Energy.Application.Services;
using EY.Energy.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));

builder.Services.AddSingleton<MongoDBContext>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoDBContext(settings.ConnectionString, settings.DatabaseName);
});

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "EnergyManagementAppCookie";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.LoginPath = "/api/account/login";
        options.AccessDeniedPath = "/api/account/accessdenied"; 
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AuthenticationServices>();

builder.Services.AddTransient<IEmailService, EmailService>();
var app = builder.Build();

builder.Services.AddControllers();

// Dans la méthode Configure de Startup.cs

app.UseAuthentication();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
