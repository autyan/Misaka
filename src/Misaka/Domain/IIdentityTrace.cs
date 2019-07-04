namespace Misaka.Domain
{
    public interface IIdentityTrace<out TId> : ITrace where TId : struct
    {
        TId Creator { get; }

        TId Modifier { get; }
    }
}
