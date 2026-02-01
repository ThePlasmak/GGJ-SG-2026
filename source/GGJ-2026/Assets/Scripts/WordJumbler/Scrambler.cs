using UnityEngine;
using System.Collections.Generic;
using System;

public class Scrambler : MonoBehaviour, IConstants
{
    private Tokenizer tokenizer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DateTime now = DateTime.Now;
        UnityEngine.Random.InitState(now.Hour * 3600 + now.Minute * 60 + now.Second);
        tokenizer = GetComponent<Tokenizer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> Scramble(string text)
    {
        if (tokenizer == null) tokenizer = GetComponent<Tokenizer>();

        List<GameObject> tokens = tokenizer.tokenize(text);

        float [] order = new float[tokens.Count];
        for (int i = 0; i < tokens.Count; i++)
        {
            order[i] = UnityEngine.Random.Range(0, 2000);
            RectTransform rectTransform = tokens[i].transform.Find("Text").GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2((i * IConstants.STEP) - (tokens.Count * IConstants.STEP / 2.0f), IConstants.DEPTH);
        }

        for (int i = 0; i < tokens.Count; i++)
        {
            for (int j = i; j < tokens.Count; j++)
            {
                if (order[i] >= order[j])
                {
                    float temp = order[j];
                    order[j] = order[i];
                    order[i] = temp;

                    
                    GameObject tempObj = tokens[i];
                    tokens[i] = tokens[j];
                    tokens[j] = tempObj;

                    Vector2 rectTemp = tokens[i].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition;
                    tokens[i].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition = tokens[j].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition;
                    tokens[j].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition = rectTemp;
                }
            }
        }
        return tokens;
    }
}
