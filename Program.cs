using million_backend.Infrastructure.Configuration;
using million_backend.Infrastructure.Persistence;
using million_backend.Domain.Interfaces;
using million_backend.Application.Interfaces;
using million_backend.Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyUseCases, PropertyUseCases>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Million Properties API", 
        Version = "v1",
        Description = "API for real estate property management",
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Million Properties API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Million Properties API";
    });
    app.UseCors("AllowAll");
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();