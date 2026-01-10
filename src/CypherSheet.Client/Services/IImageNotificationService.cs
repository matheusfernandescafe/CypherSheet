namespace CypherSheet.Client.Services;

public interface IImageNotificationService
{
    event Action<Guid>? ImageUpdated;
    event Action<Guid>? ImageRemoved;
    
    void NotifyImageUpdated(Guid characterId);
    void NotifyImageRemoved(Guid characterId);
}