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

        var characters = context.Set<Character>();

        var animeCharacters = context.Set<AnimeCharacter>();

        var mangaCharacters = context.Set<MangaCharacter>();

        var genres = context.Set<Genre>();

        var animeGenres = context.Set<AnimeGenre>();

        if (!animes.Any()) animes.AddRange(AnimeSamples.CreateAll());

        if (!mangas.Any()) mangas.AddRange(MangaSamples.CreateAll());

        if (!characters.Any()) characters.AddRange(CharacterSamples.CreateAll());

        if (!genres.Any()) genres.AddRange(GenreSamples.CreateAll());

        context.SaveChanges();

        if (!animeCharacters.Any())
            animeCharacters.AddRange(AnimeCharacterSamples.CreateAll([.. animes], [.. characters]));

        if (!mangaCharacters.Any())
            mangaCharacters.AddRange(MangaCharacterSamples.CreateAll([.. mangas], [.. characters]));

        if (!animeGenres.Any())
            animeGenres.AddRange(AnimeGenreSamples.CreateAll([.. animes], [.. genres]));

        context.SaveChanges();
    });

    options.UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var animes = context.Set<Anime>();

        var mangas = context.Set<Manga>();

        var characters = context.Set<Character>();

        var animeCharacters = context.Set<AnimeCharacter>();

        var mangaCharacter = context.Set<MangaCharacter>();

        var genres = context.Set<Genre>();

        var animeGenres = context.Set<AnimeGenre>();

        if (!await animes.AnyAsync(cancellationToken)) animes.AddRange(AnimeSamples.CreateAll());

        if (!await mangas.AnyAsync(cancellationToken)) mangas.AddRange(MangaSamples.CreateAll());

        if (!await characters.AnyAsync(cancellationToken)) characters.AddRange(CharacterSamples.CreateAll());

        if (!await genres.AnyAsync(cancellationToken)) genres.AddRange(GenreSamples.CreateAll());

        await context.SaveChangesAsync(cancellationToken);

        if (!await animeCharacters.AnyAsync(cancellationToken))
            animeCharacters.AddRange(AnimeCharacterSamples.CreateAll([.. animes], [.. characters]));

        if (!await mangaCharacter.AnyAsync(cancellationToken))
            mangaCharacter.AddRange(MangaCharacterSamples.CreateAll([.. mangas], [.. characters]));

        if (!await animeGenres.AnyAsync(cancellationToken))
            animeGenres.AddRange(AnimeGenreSamples.CreateAll([.. animes], [.. genres]));

        await context.SaveChangesAsync(cancellationToken);
    });
});

builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();

builder.Services.AddScoped<IAnimeService, AnimeService>();

builder.Services.AddScoped<IMangaRepository, MangaRepository>();

builder.Services.AddScoped<IMangaService, MangaService>();

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

builder.Services.AddScoped<ICharacterService, CharacterService>();

builder.Services.AddScoped<ISearchService, SearchService>();

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
