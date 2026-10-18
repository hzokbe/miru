using Microsoft.EntityFrameworkCore;

namespace Miru.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options);
