using UnityEngine;

public class EnemyHandMove : MonoBehaviour
{
    [SerializeField] float moveSpeed=5;
    [SerializeField] private new ParticleSystem particleSystem;
    bool switched =false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 5.3)
        {
            transform.position-=new Vector3(moveSpeed*Time.deltaTime,0,0);
        }
        if (transform.position.x <= 6 && switched == false)
        {
            switched=true;
            // print("1");
            GetComponent<ChangeEnemyHand>().SetEnemyHand();
        }
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
