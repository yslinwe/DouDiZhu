using System.Collections;
using System.Collections.Generic;
using System;
public class Env{
    private static Env instance = null;
    private static readonly object padlock = new object();
    public bool isGameOver = true;
    public bool isCallOver = false;
    public bool isGrabOver = true;
    public bool StopGame = false; 
    public static Env Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Env();
                }
                return instance;
            }
        }
    }
}