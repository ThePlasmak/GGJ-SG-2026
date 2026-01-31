using UnityEngine;

public class TurnOffOptions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ActivateOptions()
    {
        gameObject.SetActive(true);
    }
    public void DeactivateOptions()
    {
        gameObject.SetActive(false);
    }
}
