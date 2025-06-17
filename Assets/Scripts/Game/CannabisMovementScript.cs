using UnityEngine;

public class CannabisMovementScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float deadZone = -40f;
    public float acceleration = 0.1f;
    


    void Start()
    {
        
    }


    void Update()
    {
        float speed = SpeedManagerCannabisScript.currentSpeed;
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Debug.Log("Cannabis deleted");
            Destroy(gameObject);
        }
    }
}
