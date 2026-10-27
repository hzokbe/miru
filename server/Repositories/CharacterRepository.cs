using Microsoft.EntityFrameworkCore;
using Miru.Data;
using Miru.Models;

namespace Miru.Repositories;

public class CharacterRepository(AppDbContext context) : ICharacterRepository
{
    public async Task<Character?> GetByIdAsync(Guid id)
    {
        return await context.Characters.FirstOrDefaultAsync(c => c.Id == id);
    }
}
