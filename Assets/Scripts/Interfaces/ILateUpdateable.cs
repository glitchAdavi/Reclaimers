namespace Reclaimers.UpdateSystem
{
    public interface ILateUpdateable : IManagedObject
    {
        void ManagedLateUpdate();
    }
}