using UnityEngine;
using System.Collections.Generic;

public class AnswerChecker : MonoBehaviour
{
    private Timer timer;
    private bool finished;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = GetComponent<Timer>();
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> tokens = GetComponent<Swapper>().GetTokens();
        bool sorted = true;
        for (int i = 0; i < tokens.Count - 1; i++)
        {
            if (tokens[i].transform.Find("Text").GetComponent<Answer>().GetOrder() > tokens[i + 1].transform.Find("Text").GetComponent<Answer>().GetOrder())
            {
                sorted = false;
                break;
            }
        }
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].transform.Find("Text").GetComponent<Draggable>().isDragging()) sorted = false;
        }
        if (sorted && !finished)
        {
            GameStateResultEvent result = new GameStateResultEvent(true);
            Debug.Log("Sorted");
            result.Broadcast();
            finished = true;
        }
        else if (timer.GetTimeLapsed() && !finished)
        {
            finished = true;
            GameStateResultEvent result = new GameStateResultEvent(false);
            Debug.Log("Sorted");
            result.Broadcast();
        }
        else
            Debug.Log("Not Sorted");

    }
}
