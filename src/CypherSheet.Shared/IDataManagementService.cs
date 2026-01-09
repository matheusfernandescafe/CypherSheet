using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CypherSheet.Domain;

namespace CypherSheet.Shared
{
    public interface IDataManagementService
    {
        Task<List<Character>> GetAllCharactersAsync();
        Task<string> ExportCharactersAsync(List<Guid> characterIds);
        Task<ImportResult> ImportCharactersAsync(string exportData);
        Task ClearCacheAsync();
        Task DeleteAllDataAsync();
    }
}