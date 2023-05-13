using System.Collections;
using System;
using UnityEngine;

public class UItimer : MonoBehaviour
{
    public static UItimer instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void StartTime(float timeNum, Action OnTimerComplete)
    {
        StartCoroutine(startTime(timeNum,OnTimerComplete));
    }
    private IEnumerator startTime(float timeNum, Action OnTimerComplete)
    {
        yield return new WaitForSeconds(timeNum);
        //同步执行
        OnTimerComplete.Invoke();
    }
}
