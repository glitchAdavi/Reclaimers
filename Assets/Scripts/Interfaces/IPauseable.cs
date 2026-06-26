namespace Reclaimers.UpdateSystem
{
    public interface IPauseable : IManagedObject
    {
        void ManagedPause(bool paused);
    }
}

