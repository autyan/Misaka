namespace Misaka.Domain
{
    public interface IAggregateRootEvent
    {
        string Key { get; }
    }
}
