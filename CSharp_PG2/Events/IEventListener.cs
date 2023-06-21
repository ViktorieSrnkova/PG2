namespace CSharp_PG2.Events;

public interface IEventListener
{

    public void OnEvent<T>(T e) where T : IEvent;

}