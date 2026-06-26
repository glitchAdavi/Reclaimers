namespace Reclaimers.UpdateSystem
{
    public interface IFixedUpdateable : IManagedObject
    {
        void ManagedFixedUpdate();
    }
}
