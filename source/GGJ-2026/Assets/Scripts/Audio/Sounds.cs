using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Scriptable Objects/Sounds")]
public class Sounds : ScriptableObject
{
    [SerializeField] public AudioClip audioClip;
    [SerializeField] public string clipName;

    [Range(0f,1f)]
    [SerializeField] public float volume;

    [Range(.1f,3f)]
    [SerializeField] public float pitch;

    [HideInInspector]
    public AudioSource source;


}
