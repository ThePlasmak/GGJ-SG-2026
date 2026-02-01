using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Tokenizer : MonoBehaviour
{
    [SerializeField] private GameObject tokenPrefab;
    Color32[] colorList;

    void Start()
    {
        colorList = new Color32[4];
        colorList[0] = new Color32(255, 247, 138, 255);
        colorList[1] = new Color32(210, 249, 146, 255);
        colorList[2] = new Color32(174, 251, 153, 255);
        colorList[3] = new Color32(22, 255, 182, 255);
    }

    public List<GameObject> tokenize(string text)
    {
        string[] tokens = text.Split(' ');

        List<GameObject> array = new List<GameObject>();
        int tokenSize = tokens.Length;
        float [] order = new float[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
        {
            GameObject tokenObject = Instantiate(tokenPrefab);
            TMP_Text textMesh = tokenObject.transform.Find("Text").GetComponent<TMP_Text>();
            textMesh.text = tokens[i];
            tokenObject.transform.Find("Text").GetComponent<Answer>().SetOrder(i);
            textMesh.color = colorList[i];
            tokenObject.transform.GetComponent<Answer>().SetOrder(i);
            array.Add(tokenObject);
        }
        
        return array;
    }
}
