using System.Collections;
using System.Security.Cryptography;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.XR;

public class ShakeHand : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;
    string[] moves={"HighFive","HighFive","HighFive"};
    bool won=false;
    string targetMoves;
    string handType="empty";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print("!");
        targetMoves=moves[Random.Range(0,2)];
        RevealHand();
    }

    // Update is called once per frame
    // void Update()
    // {

    // }
    public void SetHandType(string hT)
    {
        handType=hT;
        print(handType);
    }
    public bool GetWin()
    {
        return won;
    }
    void RevealHand()
    {
        StartCoroutine(OpenParticles());
    }

    IEnumerator OpenParticles()
    {
        yield return new WaitForSeconds(10);
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        if (handType == "empty")
        {
            FindAnyObjectByType<TurnOffOptions>().DeactivateOptions();
        }
        if (targetMoves == handType)
        {
            won=true;
        }
        else
        {
            won=false;
        }
        yield return new WaitForSeconds(2);
        // show res

        GameStateResultEvent ev = new GameStateResultEvent(won);
        ev.Broadcast();
    }
    
}
