using UnityEngine;

public class PlayerHandMove : MonoBehaviour
{
    [SerializeField] float moveSpeed=5;
    float midX;
    void Start()
    {
        float leftBound = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x;
        float rightBound = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0)).x;
        midX=(leftBound+rightBound)/2;
    }
    void Update()
    {
        while (transform.position.x != midX)
        {
            transform.position+=new Vector3(moveSpeed*Time.deltaTime,0,0);
        }
    }
}
