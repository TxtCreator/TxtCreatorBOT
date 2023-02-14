namespace TxtCreatorBOT.Extensions;

public class EventHandlerAttribute : Attribute
{
    public EventType Type { get; }

    public EventHandlerAttribute(EventType type)
    {
        Type = type;
    }
}