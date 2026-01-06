using System.Collections.Generic;
using TG.Blazor.IndexedDB;

namespace CypherSheet.Storage
{
    public static class CypherSheetDb
    {
        public const string StoreName = "Characters";
        
        public static void Configure(DbStore store)
        {
            store.DbName = "CypherSheetDb";
            store.Version = 1;
            store.Stores.Add(new StoreSchema
            {
                Name = StoreName,
                PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = false },
                Indexes = new List<IndexSpec>
                {
                    new IndexSpec { Name = "name", KeyPath = "name", Auto = false }
                }
            });
        }
    }
}
