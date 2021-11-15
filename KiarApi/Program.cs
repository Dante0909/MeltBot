using PassionLib.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add config shared across projects
/*var sharedConfigPath = Path.GetFullPath(Path.Combine(@"../secrets/sharedconfig.json"));
builder.Configuration.AddJsonFile(sharedConfigPath);*/
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("postgres");
connectionString ??= "dummy placeholder to make EF happy when buildinging migrations";
builder.Services.AddDbContext<RunsContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<RunsContext>();
        context.Database.EnsureDeleted();
        /*context.Database.EnsureCreated();*/
        context.Database.Migrate();
        RunDbInitializer.Initialize(context);
    }
}

if (app.Environment.IsProduction())
{
    /*app.UseHsts();*/
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<RunsContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate(); // breaks if there's more than one instance running
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
