using LegiosoftTestTask.DataContext;
using LegiosoftTestTask.Extension;
using LegiosoftTestTask.Middlewares;
using LegiosoftTestTask.Services.Implementation;
using LegiosoftTestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Configuration.AddConfiguration();

builder.Services.AddAuth(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ApplicationPostgresContext>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("v1/swagger.json", "LegiosoftTestTask"));
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();  
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var context = serviceProvider.GetRequiredService<ApplicationEfContext>();
var postgresContext = serviceProvider.GetRequiredService<ApplicationPostgresContext>();
await postgresContext.AddTriggerAsync();
try
{
    await context.Database.MigrateAsync();
    //context.Database.EnsureCreated();
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();

