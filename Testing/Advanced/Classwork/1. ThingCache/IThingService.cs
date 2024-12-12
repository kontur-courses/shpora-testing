namespace Advanced.Classwork.ThingCache;

public interface IThingService
{
    bool TryRead(string thingId, out Thing value);
}