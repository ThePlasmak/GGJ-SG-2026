using UnityEngine;
using System.Collections.Generic;

public class Swapper : MonoBehaviour, IConstants
{
    private List<GameObject> tokens;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tokens = GetComponent<Scrambler>().Scramble("DAY WAS FINE THANKS.");
    }

    public List<GameObject> GetTokens()
    {
        return tokens;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentDrag = new Vector2(0,0);
        GameObject currentObj = null;

        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].transform.Find("Text").GetComponent<Draggable>().isDragging())    
            {
                currentDrag = tokens[i].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition;
                currentObj = tokens[i];
            }
        }
        if (currentObj == null)
        {
            SnapTokens();
            return;
        }

        List<GameObject> updateList = new List<GameObject>();
        int j = 0;
        Debug.Log(tokens[j].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition.x < currentDrag.x);
        while (j < tokens.Count && ((tokens[j].transform.Find("Text").GetComponent<Draggable>().isDragging()) ||
            tokens[j].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition.x < currentDrag.x))
        {
            Debug.Log("Adding");
            if (!tokens[j].transform.Find("Text").GetComponent<Draggable>().isDragging())    updateList.Add(tokens[j]);
            j++;
        }
        updateList.Add(currentObj);
        for (; j < tokens.Count; j++)
        {
            if (!tokens[j].transform.Find("Text").GetComponent<Draggable>().isDragging())    updateList.Add(tokens[j]);
        }

        tokens = updateList;
        SnapTokens();
    }

    private void SnapTokens()
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            if (!tokens[i].transform.Find("Text").GetComponent<Draggable>().isDragging())
            tokens[i].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition = new Vector2(i * IConstants.STEP - ((tokens.Count * IConstants.STEP) / 2.0f) + IConstants.OFFSET, IConstants.DEPTH);
        }
    }
}
