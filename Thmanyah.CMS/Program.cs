using Application;
using Application.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder);
var app = builder.Build();
app.UseApplicationAdminBuilder(app.Environment);
app.Run();

