using System;
using System.Collections;
using System.Collections.Generic;
public class Subject:Singleton<Subject>
{
    public enum EventName{
        GrabOver
    }
    public delegate void ObserverDelegate();
    private Dictionary<EventName,ObserverDelegate> observers = new Dictionary<EventName,ObserverDelegate>();

    public void Attach(EventName name,ObserverDelegate observer)
    {
        if(observers.ContainsKey(name))
            observers[name] += observer;
        else
            observers[name] = (observer);
    }
    public void Detach(EventName name,ObserverDelegate observer)
    {
        if(observers.ContainsKey(name))
            observers[name] -= (observer);
        else
        {
            LogOut.print("Delegate not found for name: " + name);
        }
    }
    public void Notify(EventName name)
    {
        if(observers.ContainsKey(name))
        {
            observers[name]?.Invoke();
        }
        else
        {
            LogOut.print("Delegate not found for name: " + name);
        }
    }
}