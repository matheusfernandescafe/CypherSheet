namespace CypherSheet.Domain;

public enum DataManagementError
{
    None = 0,
    InvalidFormat = 1001,
    VersionMismatch = 1002,
    SerializationError = 1003,
    DatabaseError = 1004,
    CacheError = 1005,
    UserCancelled = 1006,
    NoCharactersSelected = 1007,
    ImportDataEmpty = 1008,
    DeserializationError = 1009,
    ValidationError = 1010
}

public enum DataOperationType
{
    Export,
    Import,
    ClearCache,
    DeleteAllData,
    Validation
}