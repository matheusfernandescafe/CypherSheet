namespace CypherSheet.Client.Services;

public class ImageNotificationService : IImageNotificationService
{
    public event Action<Guid>? ImageUpdated;
    public event Action<Guid>? ImageRemoved;

    public void NotifyImageUpdated(Guid characterId)
    {
        ImageUpdated?.Invoke(characterId);
    }

    public void NotifyImageRemoved(Guid characterId)
    {
        ImageRemoved?.Invoke(characterId);
    }
}