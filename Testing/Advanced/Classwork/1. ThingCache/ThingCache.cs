using ApprovalUtilities.SimpleLogger;
using log4net;
using log4net.Core;

namespace Advanced.Classwork.ThingCache;

public class ThingCache
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(ThingCache));


    private readonly IDictionary<string, Thing> dictionary
        = new Dictionary<string, Thing>();
    private readonly IThingService thingService;

    public ThingCache(IThingService thingService)
    {
        this.thingService = thingService;
    }

    public Thing Get(string thingId)
    {
        Thing thing;
        logger.Info($"Try get by thingId=[{thingId}]");
        if (dictionary.TryGetValue(thingId, out thing))
        {
            logger.Info($"Find thing in cache");
            return thing;
        }
        if (thingService.TryRead(thingId, out thing))
        {
            logger.Info($"Find thing in service");
            dictionary[thingId] = thing;
            return thing;
        }
        logger.Info($"Not found thing");
        return null;
    }
}