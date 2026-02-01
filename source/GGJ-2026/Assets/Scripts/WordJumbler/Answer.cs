using UnityEngine;

public class Answer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int order;

    public void SetOrder(int order)
    {
        this.order = order;
    }

    public int GetOrder()
    {
        return this.order;
    }
}

