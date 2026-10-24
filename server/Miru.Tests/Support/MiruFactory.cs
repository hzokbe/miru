using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Miru.Data;
using Miru.Repositories;
using NSubstitute;

namespace Miru.Tests.Support;

public class MiruFactory : WebApplicationFactory<Program>
{
    public IAnimeRepository AnimeRepository { get; } = Substitute.For<IAnimeRepository>();

    public IMangaRepository MangaRepository { get; } = Substitute.For<IMangaRepository>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();

            services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();

            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("test"));

            services.RemoveAll<IAnimeRepository>();

            services.AddSingleton(AnimeRepository);

            services.RemoveAll<IMangaRepository>();

            services.AddSingleton(MangaRepository);
        });
    }

    public HttpClient CreateSecureClient()
    {
        return CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost")
        });
    }
}
