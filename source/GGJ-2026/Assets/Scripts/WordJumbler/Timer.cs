using UnityEngine;


public class Timer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    bool time_lapsed = false;
    float time_left;
    bool start = false;

    void Start()
    {
        time_lapsed = false;
        start = false;
        SetTime(10);
        StartTimer();
    }

    public void SetTime(float time)
    {
        time_left = time;
    }

    public void StartTimer()
    {
        start = true;
    }

    public bool GetTimeLapsed()
    {
        return time_left < 0;
    }
    // Update is called once per frame
    void Update()
    {
           if (start)
               time_left -= Time.deltaTime;
    }
}
