public interface IUpdate
{
    public void ExecuteUpdate();
}

public interface IFixedUpdate
{
    public void ExecuteFixedUpdate();
}

public interface ILateUpdate
{
    public void ExecuteLateUpdate();
}

public interface IPause
{
    public void Pause(bool paused);
}
