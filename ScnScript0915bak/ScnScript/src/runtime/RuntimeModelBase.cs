namespace ScnScript.Runtime;

public abstract class RuntimeModelBase : IDisposable
{
    public abstract void Clean();
    public abstract void Dispose();
    public abstract void Run();
    public abstract void Stop();
    public virtual void CallBack(object? result) { }
}