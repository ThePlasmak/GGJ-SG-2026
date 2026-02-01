using UnityEngine;

public class EnemyHandMove : MonoBehaviour
{
    [SerializeField] float moveSpeed=5;
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] GameObject squiggle;
    [SerializeField] float meetPointX = 5.3f;
    // [SerializeField] float switchPointX=6;

    void Start()
    {
        GetComponent<ChangeEnemyHand>().SetEnemyHand();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= meetPointX)
        {
            transform.position-=new Vector3(moveSpeed*Time.deltaTime,0,0);
            print("!");
        }
        // if (transform.position.x <= switchPointX && switched == false)
        // {
        //     switched=true;
        //     GetComponent<ChangeEnemyHand>().SetEnemyHand();
        // }
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        squiggle.SetActive(false);
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
