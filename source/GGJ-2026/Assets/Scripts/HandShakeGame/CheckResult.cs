using System.Collections;
using UnityEngine;

public class CheckResult : MonoBehaviour
{
    [SerializeField] GameObject playerHand;
    [SerializeField] GameObject playerPalm;
    [SerializeField] GameObject playerArm;
    [SerializeField] GameObject enemyHand;
    [SerializeField] GameObject enemyPalm;
    [SerializeField] GameObject enemyArm;
    [SerializeField] Sprite successHigh;
    [SerializeField] Sprite successBro;
    [SerializeField] GameObject successHand;

    [Header("Offsets for success HighFive")]
    [SerializeField] float successHighOffsetPalm;
    [SerializeField] float successHighOffsetArm;

    [SerializeField] float successHighOffsetLongArm;

    [Header("Offsets for success brofist")]
    [SerializeField] float successBroScalePalmX;
    [SerializeField] float successBroScalePalmY;
    [SerializeField] float successBroOffsetPalmY;
    [SerializeField] float successBroOffsetPalmX;
    // [SerializeField] float successBroScalePalmY;

    [Header("Offsets for success Handshake")]
    [SerializeField] float successShakeScaleX;
    [SerializeField] float successShakeScaleY;

    [SerializeField] float successShakeOffsetY;

    [SerializeField] float successShakeOffsetArmX;

    bool isWin = true;
    int selectedHand;
    public void SetSelectedHand(int x)
    {
        selectedHand=x;   
    }
    public void CheckForResult()
    {
        FindAnyObjectByType<SFXManager>().Play("HandShakeBump");
        if (selectedHand == 0 && FindAnyObjectByType<ChangeEnemyHand>().GetChosen() == 0)
        {
            // print("0");
            playerPalm.GetComponent<ChangeSprite>().Change(successHigh);
            enemyPalm.GetComponent<ChangeSprite>().Change(successHigh);
            playerPalm.transform.position+=new Vector3(0,successHighOffsetPalm,0);
            enemyPalm.transform.position+=new Vector3(0,successHighOffsetPalm,0);
            playerArm.transform.position+=new Vector3(successHighOffsetArm,0,0);
            enemyArm.transform.position-=new Vector3(successHighOffsetArm,0,0);
            playerHand.transform.position+=new Vector3(successHighOffsetLongArm,0,0);
            enemyHand.transform.position-=new Vector3(successHighOffsetLongArm,0,0);

        }else if(selectedHand == 1 && FindAnyObjectByType<ChangeEnemyHand>().GetChosen() == 1)
        {
            print("!");
            playerPalm.GetComponent<ChangeSprite>().Change(successBro);
            enemyPalm.GetComponent<ChangeSprite>().Change(successBro);
            // offset
            playerPalm.transform.position-=new Vector3(-successBroOffsetPalmX,successBroOffsetPalmY,0);
            enemyPalm.transform.position-=new Vector3(successBroOffsetPalmX,successBroOffsetPalmY,0);

            playerArm.transform.position+=new Vector3(successBroOffsetPalmX,0,0);
            enemyArm.transform.position+=new Vector3(-successBroOffsetPalmX,0,0);

            // scale
            playerPalm.transform.localScale-=new Vector3(successBroScalePalmX,-successBroScalePalmY,0);
            enemyPalm.transform.localScale+=new Vector3(successBroScalePalmX,successBroScalePalmY,0);

        }else if(selectedHand == 2 && FindAnyObjectByType<ChangeEnemyHand>().GetChosen() == 2)
        {
            Destroy(enemyPalm);
            Destroy(playerPalm);
            successHand.SetActive(true);
            successHand.transform.localScale+=new Vector3(successShakeScaleX,successShakeScaleY,0);
            successHand.transform.position-=new Vector3(0,successShakeOffsetY,0);

            playerArm.transform.position+=new Vector3(successShakeOffsetArmX,0,0);
            enemyArm.transform.position-=new Vector3(successShakeOffsetArmX,0,0);
        }
        else
        {
            isWin=false;
        }

        GameStateResultEvent ev = new GameStateResultEvent(isWin);
        ev.Broadcast();
    }

}
