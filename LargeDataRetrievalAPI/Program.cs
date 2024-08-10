using LargeDataRetrievalAPI.Data;
using LargeDataRetrievalAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LargeDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Logging.AddConsole();

var app = builder.Build();

//Seed the database
using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LargeDataContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    context.Database.Migrate(); // Ensure database is created and migrated to latest version
    await context.SeedDataAsync(); // Call the extension method to seed data
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
