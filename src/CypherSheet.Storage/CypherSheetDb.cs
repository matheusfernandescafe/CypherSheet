using System.Collections.Generic;
using TG.Blazor.IndexedDB;

namespace CypherSheet.Storage
{
    public static class CypherSheetDb
    {
        public const string StoreName = "Characters";
        public const string ImageStoreName = "character-images";
        
        public static void Configure(DbStore store)
        {
            store.DbName = "CypherSheetDb";
            store.Version = 2; // Incrementar versão para adicionar novo store
            
            // Store para personagens
            store.Stores.Add(new StoreSchema
            {
                Name = StoreName,
                PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = false },
                Indexes = new List<IndexSpec>
                {
                    new IndexSpec { Name = "name", KeyPath = "name", Auto = false }
                }
            });
            
            // Store para imagens de personagens
            store.Stores.Add(new StoreSchema
            {
                Name = ImageStoreName,
                PrimaryKey = new IndexSpec { Name = "characterId", KeyPath = "characterId", Auto = false },
                Indexes = new List<IndexSpec>
                {
                    new IndexSpec { Name = "uploadDate", KeyPath = "uploadDate", Auto = false }
                }
            });
        }
    }
}
