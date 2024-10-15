using Application;
using Infrastructure;
using Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

builder.Services.AddControllers().AddApplicationPart(presentationAssembly);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
    await seeder.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();