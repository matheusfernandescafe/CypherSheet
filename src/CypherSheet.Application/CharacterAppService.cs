using CypherSheet.Domain;
using CypherSheet.Storage;

namespace CypherSheet.Application;

public class CharacterAppService : ICharacterAppService
{
    private readonly ICharacterRepository _repository;

    public CharacterAppService(ICharacterRepository repository)
    {
        _repository = repository;
    }

    public Task<Character?> GetCharacterAsync(Guid id)
        => _repository.GetCharacterAsync(id);

    public Task<List<Character>> GetAllCharactersAsync()
        => _repository.GetAllCharactersAsync();

    public Task SaveCharacterAsync(Character character)
        => _repository.SaveCharacterAsync(character);

    public Task DeleteCharacterAsync(Guid id)
        => _repository.DeleteCharacterAsync(id);

    public Task CreateCharacterAsync(Character character)
        => _repository.SaveCharacterAsync(character);
}
