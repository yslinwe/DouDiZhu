using System.Collections;
using System;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    private float m_duration;
    private float m_elapsedTime;
    private bool m_isRunning = false;
    private Action m_onTimerComplete = null;
    private TextMeshProUGUI m_timertext;
    private ShakeEffect m_shake;
    private bool first = false;
    public static Timer instance;

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
    public void StartTimer(float duration, Action onTimerComplete)
    {
        if (!m_isRunning)
        {
            first = false;
            m_shake = m_timertext.GetComponentInParent<ShakeEffect>();
            m_duration = duration;
            m_onTimerComplete = onTimerComplete;
            StartCoroutine(TimerCoroutine());
        }
    }
    public bool IsRunning { get { return m_isRunning; } }
    public string ElapsedTime { get { return m_elapsedTime.ToString("F0").PadLeft(2, '0'); } }
    public TextMeshProUGUI Timertext { set { m_timertext=value; } }
    public void StopTimer()
    {
        if (m_isRunning)
        {
            // Debug.Log("结束计时器");
            StopCoroutine(TimerCoroutine());
            m_isRunning = false;
        }
    }

    private IEnumerator TimerCoroutine()
    {
        m_isRunning = true;
        m_elapsedTime = 0;

        while (m_elapsedTime < m_duration)
        {
            m_elapsedTime += Time.deltaTime;
            m_timertext.text = m_elapsedTime.ToString("F0").PadLeft(2, '0');
            if(m_duration-m_elapsedTime<2&&!first)
            {
                if(m_shake)
                {
                    first = true;
                    m_shake.Shake();
                }               
            }
            yield return null;
        }
        // Debug.Log("计时器完成并且执行任务");
        m_isRunning = false;
        m_onTimerComplete?.Invoke();
    }
}
