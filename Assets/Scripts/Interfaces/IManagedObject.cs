namespace Reclaimers.UpdateSystem
{
    public interface IManagedObject { }

    public static class IUpdatableExtensions
    {
        public static void Register(this IManagedObject obj)
        {
            UpdateSystem.Instance.Register(obj);
        }

        public static void Unregister(this IManagedObject obj)
        {
            UpdateSystem.Instance.Unregister(obj);
        }
    }
}

