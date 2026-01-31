using UnityEngine;
using System.Collections.Generic;

public class AnswerChecker : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (sorted)
            Debug.Log("Sorted");
        else
            Debug.Log("Not Sorted");
    }
}
