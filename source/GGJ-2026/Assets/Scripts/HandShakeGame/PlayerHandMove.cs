using UnityEngine;

public class PlayerHandMove : MonoBehaviour
{
    [SerializeField] float moveSpeed=5;
    [SerializeField] GameObject options;
    float midX;
    void Start()
    {
    }
    void Update()
    {
        if (transform.position.x <= -5.3)
        {
            transform.position+=new Vector3(moveSpeed*Time.deltaTime,0,0);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(options);
        FindAnyObjectByType<CheckResult>().CheckForResult();
    }
}
