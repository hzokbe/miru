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

        var characters = context.Set<Character>();

        var animeCharacters = context.Set<AnimeCharacter>();

        var mangaCharacters = context.Set<MangaCharacter>();

        if (!animes.Any()) animes.AddRange(AnimeSeedData.Create());

        if (!mangas.Any()) mangas.AddRange(MangaSeedData.Create());

        if (!genres.Any()) genres.AddRange(GenreSeedData.Create());

        if (!characters.Any()) characters.AddRange(CharacterSeedData.Create());

        context.SaveChanges();

        if (!animeGenres.Any()) animeGenres.AddRange(AnimeGenreSeedData.Create([.. animes], [.. genres]));

        if (!mangaGenres.Any()) mangaGenres.AddRange(MangaGenreSeedData.Create([.. mangas], [.. genres]));

        if (!animeCharacters.Any())
            animeCharacters.AddRange(AnimeCharacterSeedData.Create([.. animes], [.. characters]));

        if (!mangaCharacters.Any())
            mangaCharacters.AddRange(MangaCharacterSeedData.Create([.. mangas], [.. characters]));

        context.SaveChanges();
    });

    options.UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var animes = context.Set<Anime>();

        var mangas = context.Set<Manga>();

        var genres = context.Set<Genre>();

        var animeGenres = context.Set<AnimeGenre>();

        var mangaGenres = context.Set<MangaGenre>();

        var characters = context.Set<Character>();

        var animeCharacters = context.Set<AnimeCharacter>();

        var mangaCharacters = context.Set<MangaCharacter>();

        if (!await animes.AnyAsync(cancellationToken)) animes.AddRange(AnimeSeedData.Create());

        if (!await mangas.AnyAsync(cancellationToken)) mangas.AddRange(MangaSeedData.Create());

        if (!await genres.AnyAsync(cancellationToken)) genres.AddRange(GenreSeedData.Create());

        if (!await characters.AnyAsync(cancellationToken)) characters.AddRange(CharacterSeedData.Create());

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

        if (!await animeCharacters.AnyAsync(cancellationToken))
            animeCharacters.AddRange(
                AnimeCharacterSeedData.Create(
                    await animes.ToListAsync(cancellationToken),
                    await characters.ToListAsync(cancellationToken)
                )
            );

        if (!await mangaCharacters.AnyAsync(cancellationToken))
            mangaCharacters.AddRange(
                MangaCharacterSeedData.Create(
                    await mangas.ToListAsync(cancellationToken),
                    await characters.ToListAsync(cancellationToken)
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
