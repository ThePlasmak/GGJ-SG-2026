using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Tokenizer : MonoBehaviour
{
    [SerializeField] private GameObject tokenPrefab;

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
            tokenObject.transform.GetComponent<Answer>().SetOrder(i);
            array.Add(tokenObject);
        }
        return array;
    }
}
