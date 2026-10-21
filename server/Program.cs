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

        var mangas = context.Set<Manga>();

        var genres = context.Set<Genre>();

        var animeGenres = context.Set<AnimeGenre>();

        var mangaGenres = context.Set<MangaGenre>();

        if (!animes.Any()) animes.AddRange(AnimeSeedData.Create());

        if (!mangas.Any()) mangas.AddRange(MangaSeedData.Create());

        if (!genres.Any()) genres.AddRange(GenreSeedData.Create());

        context.SaveChanges();

        if (!animeGenres.Any()) animeGenres.AddRange(AnimeGenreSeedData.Create([.. animes], [.. genres]));

        if (!mangaGenres.Any()) mangaGenres.AddRange(MangaGenreSeedData.Create([.. mangas], [.. genres]));

        context.SaveChanges();
    });

    options.UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var animes = context.Set<Anime>();

        var mangas = context.Set<Manga>();

        var genres = context.Set<Genre>();

        var animeGenres = context.Set<AnimeGenre>();

        var mangaGenres = context.Set<MangaGenre>();

        if (!await animes.AnyAsync(cancellationToken)) animes.AddRange(AnimeSeedData.Create());

        if (!await mangas.AnyAsync(cancellationToken)) mangas.AddRange(MangaSeedData.Create());

        if (!await genres.AnyAsync(cancellationToken)) genres.AddRange(GenreSeedData.Create());

        await context.SaveChangesAsync(cancellationToken);

        if (!await animeGenres.AnyAsync(cancellationToken))
            animeGenres.AddRange(
                AnimeGenreSeedData.Create(
                    await animes.ToListAsync(cancellationToken),
                    await genres.ToListAsync(cancellationToken)
                )
            );

        if (!await mangaGenres.AnyAsync(cancellationToken))
            mangaGenres.AddRange(
                MangaGenreSeedData.Create(
                    await mangas.ToListAsync(cancellationToken),
                    await genres.ToListAsync(cancellationToken)
                )
            );

        await context.SaveChangesAsync(cancellationToken);
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

builder.Services.AddScoped<IMangaRepository, MangaRepository>();

builder.Services.AddScoped<IMangaService, MangaService>();

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
