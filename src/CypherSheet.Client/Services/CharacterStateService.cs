using CypherSheet.Application;
using CypherSheet.Domain;

namespace CypherSheet.Client.Services
{
    public class CharacterStateService
    {
        private readonly ICharacterAppService _appService;
        private Character? _character;

        public event Action? OnChange;

        public CharacterStateService(ICharacterAppService appService)
        {
            _appService = appService;
        }

        public Character? Character => _character;

        public async Task LoadCharacterAsync(Guid id)
        {
            _character = await _appService.GetCharacterAsync(id);
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
                await _appService.SaveCharacterAsync(_character);
                NotifyStateChanged();
            }
        }

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
