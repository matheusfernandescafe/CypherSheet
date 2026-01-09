using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CypherSheet.Domain;
using CypherSheet.Shared;
using CypherSheet.Storage;

namespace CypherSheet.Client.Services
{
    public class DataManagementService : IDataManagementService
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly ICacheService _cacheService;

        public DataManagementService(ICharacterRepository characterRepository, ICacheService cacheService)
        {
            _characterRepository = characterRepository;
            _cacheService = cacheService;
        }

        public async Task<List<Character>> GetAllCharactersAsync()
        {
            try
            {
                return await _characterRepository.GetAllCharactersAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao obter personagens: {ex.Message}", ex);
            }
        }

        public async Task<string> ExportCharactersAsync(List<Guid> characterIds)
        {
            if (characterIds == null || !characterIds.Any())
            {
                throw new ArgumentException("Pelo menos um personagem deve ser selecionado para exportação.");
            }

            try
            {
                var allCharacters = await _characterRepository.GetAllCharactersAsync();
                var selectedCharacters = allCharacters.Where(c => characterIds.Contains(c.Id)).ToList();

                if (selectedCharacters.Count != characterIds.Count)
                {
                    var missingIds = characterIds.Except(selectedCharacters.Select(c => c.Id)).ToList();
                    throw new InvalidOperationException($"Personagens não encontrados: {string.Join(", ", missingIds)}");
                }

                var exportFormat = new ExportFormat
                {
                    Metadata = new ExportMetadata
                    {
                        AppVersion = AppVersion.Version,
                        ExportDate = DateTime.UtcNow,
                        CharacterCount = selectedCharacters.Count,
                        FormatVersion = "1.0"
                    },
                    Characters = selectedCharacters
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                return JsonSerializer.Serialize(exportFormat, options);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erro na serialização dos dados: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro durante a exportação: {ex.Message}", ex);
            }
        }

        public async Task<ImportResult> ImportCharactersAsync(string exportData)
        {
            var result = new ImportResult();

            if (string.IsNullOrWhiteSpace(exportData))
            {
                result.Message = "Dados de importação não podem estar vazios.";
                return result;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var exportFormat = JsonSerializer.Deserialize<ExportFormat>(exportData, options);

                if (exportFormat == null)
                {
                    result.Message = "Formato de dados inválido.";
                    return result;
                }

                // Validar metadados
                if (exportFormat.Metadata == null)
                {
                    result.Message = "Metadados de exportação não encontrados.";
                    return result;
                }

                // Verificar compatibilidade de versão
                var versionCheck = ValidateVersion(exportFormat.Metadata.AppVersion);
                if (!versionCheck.IsCompatible)
                {
                    result.VersionMismatch = true;
                    if (versionCheck.IsBlocking)
                    {
                        result.Message = $"Versão incompatível. Dados exportados da versão {exportFormat.Metadata.AppVersion}, aplicação atual: {AppVersion.Version}";
                        return result;
                    }
                    else
                    {
                        result.Warnings.Add($"Aviso de compatibilidade: Dados da versão {exportFormat.Metadata.AppVersion}, aplicação atual: {AppVersion.Version}");
                    }
                }

                // Validar personagens
                if (exportFormat.Characters == null || !exportFormat.Characters.Any())
                {
                    result.Message = "Nenhum personagem encontrado nos dados de importação.";
                    return result;
                }

                // Importar personagens
                var importedCount = 0;
                var existingCharacters = await _characterRepository.GetAllCharactersAsync();
                var existingIds = existingCharacters.Select(c => c.Id).ToHashSet();

                foreach (var character in exportFormat.Characters)
                {
                    try
                    {
                        // Se o personagem já existe, gerar novo ID
                        if (existingIds.Contains(character.Id))
                        {
                            character.Id = Guid.NewGuid();
                            character.Name += " (Importado)";
                            result.Warnings.Add($"Personagem duplicado renomeado para: {character.Name}");
                        }

                        await _characterRepository.SaveCharacterAsync(character);
                        importedCount++;
                    }
                    catch (Exception ex)
                    {
                        result.Warnings.Add($"Erro ao importar personagem '{character.Name}': {ex.Message}");
                    }
                }

                result.Success = importedCount > 0;
                result.ImportedCount = importedCount;
                result.Message = importedCount > 0 
                    ? $"Importação concluída com sucesso. {importedCount} personagem(ns) importado(s)."
                    : "Nenhum personagem foi importado.";

                return result;
            }
            catch (JsonException ex)
            {
                result.Message = $"Erro no formato JSON: {ex.Message}";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = $"Erro durante a importação: {ex.Message}";
                return result;
            }
        }

        public async Task ClearCacheAsync()
        {
            try
            {
                await _cacheService.ClearApplicationCacheAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao limpar cache: {ex.Message}", ex);
            }
        }

        public async Task DeleteAllDataAsync()
        {
            try
            {
                var allCharacters = await _characterRepository.GetAllCharactersAsync();
                
                foreach (var character in allCharacters)
                {
                    await _characterRepository.DeleteCharacterAsync(character.Id);
                }

                await _cacheService.ClearApplicationCacheAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao deletar todos os dados: {ex.Message}", ex);
            }
        }

        private (bool IsCompatible, bool IsBlocking) ValidateVersion(string exportVersion)
        {
            if (string.IsNullOrWhiteSpace(exportVersion))
            {
                return (true, false); // Assumir compatibilidade se não há versão
            }

            try
            {
                var currentVersion = Version.Parse(AppVersion.Version);
                var dataVersion = Version.Parse(exportVersion);

                // Versão maior é incompatível e bloqueia
                if (dataVersion.Major > currentVersion.Major)
                {
                    return (false, true);
                }

                // Versão menor é compatível mas com aviso
                if (dataVersion.Major < currentVersion.Major || 
                    (dataVersion.Major == currentVersion.Major && dataVersion.Minor < currentVersion.Minor))
                {
                    return (true, false);
                }

                // Mesma versão ou patch diferente é totalmente compatível
                return (true, false);
            }
            catch (Exception)
            {
                // Se não conseguir parsear a versão, assumir compatibilidade com aviso
                return (true, false);
            }
        }
    }
}