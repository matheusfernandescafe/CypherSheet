using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CypherSheet.Domain;
using TG.Blazor.IndexedDB;

namespace CypherSheet.Storage
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly IndexedDBManager _dbManager;

        public CharacterRepository(IndexedDBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public async Task<List<Character>> GetAllCharactersAsync()
        {
            try
            {
                return await _dbManager.GetRecords<Character>(CypherSheetDb.StoreName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting characters: {ex.Message}");
                return new List<Character>();
            }
        }

        public async Task<Character> GetCharacterAsync(Guid id)
        {
             try
            {
                return await _dbManager.GetRecordById<Guid, Character>(CypherSheetDb.StoreName, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting character {id}: {ex.Message}");
                return null;
            }
        }

        public async Task SaveCharacterAsync(Character character)
        {
            var existing = await GetCharacterAsync(character.Id);
            
            var record = new StoreRecord<Character>
            {
                Storename = CypherSheetDb.StoreName,
                Data = character
            };

            if (existing != null)
            {
                await _dbManager.UpdateRecord(record);
            }
            else
            {
                await _dbManager.AddRecord(record);
            }
        }

        public async Task DeleteCharacterAsync(Guid id)
        {
            await _dbManager.DeleteRecord(CypherSheetDb.StoreName, id);
        }
    }
}
