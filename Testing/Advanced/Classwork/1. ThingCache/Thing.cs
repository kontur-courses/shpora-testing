namespace Advanced.Classwork.ThingCache;

public class Thing
{
    public string ThingId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public Thing(string thingId)
    {
        ThingId = thingId;
    }

}