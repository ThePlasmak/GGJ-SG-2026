using System;
using UnityEngine;

public class ChangeEnemyHand : MonoBehaviour
{
    [SerializeField] Sprite[] arr;
    [SerializeField] GameObject enemyHand;
    int chosen;
    public void SetEnemyHand()
    {
        chosen = UnityEngine.Random.Range(0,3);

        enemyHand.GetComponent<SpriteRenderer>().sprite=arr[chosen];
        // print("5")
    }
    public int GetChosen()
    {
        return chosen;
    }
}
