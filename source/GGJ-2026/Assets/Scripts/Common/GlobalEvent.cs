using System.Collections.Generic;

public class GlobalEvent<EventType> where EventType : class
{
    public delegate void EventCallback(EventType eventData);

    private static List<EventCallback> Callbacks { get; set; } = new List<EventCallback>();

    public static void AddListener(EventCallback callback)
    {
        Callbacks.Add(callback);
    }

    public static void RemoveListener(EventCallback callback)
    {
        Callbacks.Remove(callback);
    }

    public static void ClearListeners()
    {
        Callbacks.Clear();
    }

    public static void Broadcast(EventType eventData)
    {
        if(eventData == null)
        {
            return;
        }

        foreach(EventCallback callback in Callbacks)
        {
            callback.Invoke(eventData);
        }
    }

    public void Broadcast()
    {
        Broadcast(this as EventType);
    }
}