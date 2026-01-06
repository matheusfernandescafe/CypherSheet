using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CypherSheet.Domain;

namespace CypherSheet.Storage
{
    public interface ICharacterRepository
    {
        Task SaveCharacterAsync(Character character);
        Task<Character> GetCharacterAsync(Guid id);
        Task<List<Character>> GetAllCharactersAsync();
        Task DeleteCharacterAsync(Guid id);
    }
}
