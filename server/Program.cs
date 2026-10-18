using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Miru.Data;
using Miru.Data.Seeding;
using Miru.Models;
using Miru.Repositories;
using Miru.Services;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod())
);

var connectionString = builder.Configuration.GetConnectionString("Default") ??
                       throw new InvalidOperationException("ConnectionStrings:Default must be configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);

    options.UseSeeding((context, _) =>
    {
        var animes = context.Set<Anime>();

        if (animes.Any()) return;

        animes.AddRange(AnimeSeedData.Create());

        context.SaveChanges();
    });

    options.UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var animes = context.Set<Anime>();

        if (!await animes.AnyAsync(cancellationToken))
        {
            animes.AddRange(AnimeSeedData.Create());

            await context.SaveChangesAsync(cancellationToken);
        }
    });
});

builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddOpenApi();

builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();

builder.Services.AddScoped<IAnimeService, AnimeService>();

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment()) app.MapOpenApi();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
