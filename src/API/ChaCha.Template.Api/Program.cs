using ChaCha.Notification.Api;
using ChaCha.Security.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddSecurityModuleDependencyInjection();
builder.Services.AddNotificationModule();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapControllers();
app.MapDefaultControllerRoute();

app.UseHttpsRedirection();

app.Run();