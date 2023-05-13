using System.Collections;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private int currentTaskIndex = 0;
    private bool isTaskCompleted = true;
    
    void Start()
    {
        // 开始执行任务
        StartCoroutine(RunTasks());
    }
    
    private IEnumerator RunTasks()
    {
        while (currentTaskIndex < 3)
        {
            // 如果任务已经完成，就进入下一个任务
            if (isTaskCompleted)
            {
                isTaskCompleted = false;
                currentTaskIndex++;
                Debug.Log("开始执行任务" + currentTaskIndex);
                yield return StartCoroutine(ExecuteTask());
            }
            yield return null;
        }
        Debug.Log("所有任务完成");
    }
    
    private IEnumerator ExecuteTask()
    {
        float elapsedTime = 0f;
        float taskTimeLimit = 12f; // 每个任务的时间限制为12秒
        
        while (elapsedTime < taskTimeLimit)
        {
            // 模拟任务执行过程
            elapsedTime += Time.deltaTime;
            taskTimeLimit = 0;
            Debug.Log("执行任务" + currentTaskIndex + "，已用时间：" + elapsedTime);
            yield return null;
        }
        
        // 任务完成
        isTaskCompleted = true;
        Debug.Log("任务" + currentTaskIndex + "完成");
    }
}
