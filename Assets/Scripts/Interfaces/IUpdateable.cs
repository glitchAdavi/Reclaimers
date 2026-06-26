namespace Reclaimers.UpdateSystem
{
    public interface IUpdateable : IManagedObject
    {
        void ManagedUpdate();
    }
}

