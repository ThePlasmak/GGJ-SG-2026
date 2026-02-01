using System.Collections;
using UnityEngine;

public class CheckResult : MonoBehaviour
{
    [SerializeField] GameObject playerHand;
    [SerializeField] GameObject playerPalm;
    [SerializeField] GameObject enemyHand;
    [SerializeField] GameObject enemyPalm;
    [SerializeField] Sprite successHigh;
    [SerializeField] Sprite successBro;
    [SerializeField] GameObject successHand;
    bool isWin = true;
    int selectedHand;
    public void SetSelectedHand(int x)
    {
        selectedHand=x;   
    }
    public void CheckForResult()
    {
        if (selectedHand == 0 && FindAnyObjectByType<ChangeEnemyHand>().GetChosen() == 0)
        {
            // print("0");
            playerPalm.GetComponent<ChangeSprite>().Change(successHigh);
            enemyPalm.GetComponent<ChangeSprite>().Change(successHigh);
            // playerPalm.GetComponent<SpriteRenderer>().sprite=successHigh;
            // enemyPalm.GetComponent<SpriteRenderer>().sprite=successHigh;
            // print("@");
        }else if(selectedHand == 1 && FindAnyObjectByType<ChangeEnemyHand>().GetChosen() == 1)
        {
            print("!");
            playerPalm.GetComponent<ChangeSprite>().Change(successBro);
            enemyPalm.GetComponent<ChangeSprite>().Change(successBro);
        }else if(selectedHand == 2 && FindAnyObjectByType<ChangeEnemyHand>().GetChosen() == 2)
        {
            Destroy(enemyPalm);
            Destroy(playerPalm);
            successHand.SetActive(true);
        }
        else
        {
            isWin=false;
        }

        StartCoroutine(HandleResult());
    }
    IEnumerator HandleResult()
    {
        yield return new WaitForSeconds(5);
        GameStateResultEvent ev = new GameStateResultEvent(isWin);
        ev.Broadcast();
    }
}
