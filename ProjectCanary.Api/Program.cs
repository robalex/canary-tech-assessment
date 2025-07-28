using Microsoft.EntityFrameworkCore;
using ProjectCanary.BusinessLogic.Services;
using ProjectCanary.BusinessLogic.Services.Implementations;
using ProjectCanary.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddScoped<IEmissionsService, EmissionsService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalReactHost", policy => {
        if (builder.Environment.IsDevelopment()) {
            policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        }
    });
});

builder.Services.AddDbContext<ProjectCanaryDbContext>(options =>
{
    options
        .UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention();
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalReactHost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
