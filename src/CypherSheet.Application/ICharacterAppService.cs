using CypherSheet.Domain;

namespace CypherSheet.Application;

public interface ICharacterAppService
{
    Task<Character?> GetCharacterAsync(Guid id);
    Task<List<Character>> GetAllCharactersAsync();
    Task SaveCharacterAsync(Character character);
    Task DeleteCharacterAsync(Guid id);
    Task CreateCharacterAsync(Character character);
}
