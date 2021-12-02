using PassionLib.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

var connectionString = builder.Configuration.GetConnectionString("postgres");
connectionString ??= @"host=localhost;port=5432;Database=aecrdb;User ID=postgres;Password=DefaultLUL;";
builder.Services.AddDbContext<RunsContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
//else
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

var doDbRecreation = bool.Parse(Environment.GetEnvironmentVariable("DO_DB_RECREATION") ?? "false");
if (doDbRecreation)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<RunsContext>();
        context.Database.EnsureDeleted();
        context.Database.Migrate();
        await RunDbInitializer.Initialize(context);
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

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
