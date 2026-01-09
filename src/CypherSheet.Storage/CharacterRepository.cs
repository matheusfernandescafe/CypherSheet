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

        public async Task<Character?> GetCharacterAsync(Guid id)
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
            
            // Também remover a imagem associada, se existir
            try
            {
                await DeleteCharacterImageAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting character image for {id}: {ex.Message}");
                // Não falhar a operação se não conseguir deletar a imagem
            }
        }

        public async Task SaveCharacterImageAsync(Guid characterId, byte[] imageData, string fileName, string contentType)
        {
            try
            {
                var imageRecord = new CharacterImageData
                {
                    CharacterId = characterId,
                    ImageData = imageData,
                    FileName = fileName,
                    ContentType = contentType,
                    FileSize = imageData.Length,
                    UploadDate = DateTime.UtcNow
                };

                var storeRecord = new StoreRecord<CharacterImageData>
                {
                    Storename = CypherSheetDb.ImageStoreName,
                    Data = imageRecord
                };

                // Verificar se já existe uma imagem para este personagem
                var existingImage = await GetCharacterImageDataAsync(characterId);
                
                if (existingImage != null)
                {
                    await _dbManager.UpdateRecord(storeRecord);
                }
                else
                {
                    await _dbManager.AddRecord(storeRecord);
                }

                // Atualizar os metadados no personagem
                var character = await GetCharacterAsync(characterId);
                if (character != null)
                {
                    character.ImageFileName = fileName;
                    character.ImageContentType = contentType;
                    character.ImageUploadDate = imageRecord.UploadDate;
                    await SaveCharacterAsync(character);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving character image for {characterId}: {ex.Message}");
                throw;
            }
        }

        public async Task<byte[]?> GetCharacterImageAsync(Guid characterId)
        {
            try
            {
                var imageData = await GetCharacterImageDataAsync(characterId);
                return imageData?.ImageData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting character image for {characterId}: {ex.Message}");
                return null;
            }
        }

        public async Task DeleteCharacterImageAsync(Guid characterId)
        {
            try
            {
                await _dbManager.DeleteRecord(CypherSheetDb.ImageStoreName, characterId);
                
                // Limpar os metadados no personagem
                var character = await GetCharacterAsync(characterId);
                if (character != null)
                {
                    character.ImageFileName = null;
                    character.ImageContentType = null;
                    character.ImageUploadDate = null;
                    await SaveCharacterAsync(character);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting character image for {characterId}: {ex.Message}");
                throw;
            }
        }

        private async Task<CharacterImageData?> GetCharacterImageDataAsync(Guid characterId)
        {
            try
            {
                return await _dbManager.GetRecordById<Guid, CharacterImageData>(CypherSheetDb.ImageStoreName, characterId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting character image data for {characterId}: {ex.Message}");
                return null;
            }
        }
    }
}
