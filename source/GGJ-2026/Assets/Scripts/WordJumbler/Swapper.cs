using UnityEngine;
using System.Collections.Generic;

public class Swapper : MonoBehaviour, IConstants
{
    private List<GameObject> tokens;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tokens = GetComponent<Scrambler>().Scramble("Hi this is a test");
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
        List<GameObject> updateList = new List<GameObject>();

        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].transform.Find("Text").GetComponent<Draggable>().isDragging())    
            {
                currentDrag = tokens[i].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition;
                currentObj = tokens[i];
            }
            else
                updateList.Add(tokens[i]);
        }
        if (currentObj == null)
        {
            Debug.Log("No dragging");
            SnapTokens();
            return;
        }

        bool insert = false;
        for (int i = 0; i < updateList.Count; i++)
        {
            if ((i == 0 && updateList[i].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition.x > currentDrag.x) ||
                (i < updateList.Count - 1 && i > 0 &&
                updateList[i].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition.x < currentDrag.x &&
                updateList[i + 1].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition.x > currentDrag.x) ||
                (i == updateList.Count - 1 && updateList[i].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition.x < currentDrag.x))
            {
                Debug.Log("Inserting into " + i);
                insert = true;
                updateList.Insert(i, currentObj);
                break;
            }
        }
        if (insert) tokens = updateList;
        else updateList.Add(currentObj);
        SnapTokens();
    }

    private void SnapTokens()
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            if (!tokens[i].transform.Find("Text").GetComponent<Draggable>().isDragging())
            tokens[i].transform.Find("Text").GetComponent<RectTransform>().anchoredPosition = new Vector2(i * IConstants.STEP - ((tokens.Count * IConstants.STEP) / 2.0f), IConstants.DEPTH);
        }
    }
}
