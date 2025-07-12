using Domain.Commons;
using Infrastructure;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices(builder).AddAPIInfrastructureServices(builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();

    options.AddPolicy("AllowClientPolicy", policy =>
    {
        if (corsSettings.AllowAnyMethod)
            policy.AllowAnyMethod();

        if (corsSettings.AllowAnyHeader)
            policy.AllowAnyHeader();

        if (corsSettings.AllowCredentials)
            policy.AllowCredentials();

        if (corsSettings.AllowAnyOrigin)
        {
            policy.AllowAnyOrigin();
        }
        else if (corsSettings.Origins?.Length > 0)
        {
            policy.WithOrigins(corsSettings.Origins)
                  .SetIsOriginAllowedToAllowWildcardSubdomains(); // Optional
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowClientPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
