using CypherSheet.Domain;
using CypherSheet.Storage;
using System;
using System.Threading.Tasks;

namespace CypherSheet.Client.Services
{
    public class CharacterStateService
    {
        private readonly ICharacterRepository _repository;
        private Character? _character;

        public event Action? OnChange;

        public CharacterStateService(ICharacterRepository repository)
        {
            _repository = repository;
        }

        public Character? Character => _character;

        public async Task LoadCharacterAsync(Guid id)
        {
            _character = await _repository.GetCharacterAsync(id);
            NotifyStateChanged();
        }

        public void SetCharacter(Character character)
        {
            _character = character;
            NotifyStateChanged();
        }

        public async Task SaveCharacterAsync()
        {
            if (_character != null)
            {
                await _repository.SaveCharacterAsync(_character);
                NotifyStateChanged();
            }
        }

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
