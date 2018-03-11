namespace Svelto.ECS.Example.Survive
{
    public interface IDestroyComponent
    {
        DispatchOnChange<bool> destroyed { get; }
    }
}