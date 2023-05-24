using  UnityEngine;
public class LogOut:MonoBehaviour
{
    private static LogOut instance = null;
    private static readonly object padlock = new object();

    private LogOut() { }

    public static LogOut Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new LogOut();
                }
                return instance;
            }
        }
    }

    public void Print(string message)
    {
        Debug.Log(message);
    }
}
