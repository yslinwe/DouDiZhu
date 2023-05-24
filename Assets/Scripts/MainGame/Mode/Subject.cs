using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Subject:Singleton<Subject>
{
    public enum EventName{
        GrabOver,
        EnableButton,
        DealCardsFirstOver,
        SetShowInfo,
        GameOver
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
            observers[name] -= observer;
        else
        {
            LogOut.print("Delegate not found for name: " + name);
        }
    }
    public void Notify(EventName name,bool del=false)
    {
        if(observers.ContainsKey(name))
        {
            if(del)
            {
                Delegate[] eventList = observers[name]?.GetInvocationList();
                if (eventList != null&&eventList.Length>0)
                {
                    foreach (ObserverDelegate action in eventList)
                    {        
                        action.Invoke();
                        Detach(name,action);
                        break;
                    }
                }
            }
            else
                observers[name]?.Invoke();
            
        }
        else
        {
            LogOut.print("Delegate not found for name: " + name);
        }
    }
    public void Clear()
    {
        foreach (EventName name in Enum.GetValues(typeof(EventName)))
        {
            observers[name] = null; 
        }
    }
}