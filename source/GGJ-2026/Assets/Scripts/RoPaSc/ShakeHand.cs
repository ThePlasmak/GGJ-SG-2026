using System.Collections;
using UnityEngine;

public class ShakeHand : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RevealHand();
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    void RevealHand()
    {
        StartCoroutine(OpenParticles());
    }

    IEnumerator OpenParticles()
    {
        yield return new WaitForSeconds(10);
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
    
}
