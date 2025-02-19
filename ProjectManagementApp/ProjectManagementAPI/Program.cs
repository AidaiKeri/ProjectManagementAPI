using ProjectManagement.DAL.DataAccess;
using ProjectManagement.BLL.Interfaces;
using ProjectManagement.BLL.Services;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebApiDbContext>();

builder.Services.AddScoped<IEmployee, EmployeeService>();
builder.Services.AddScoped<IProject, ProjectService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WebApiDbContext>();
    dbContext.Database.Migrate();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();



