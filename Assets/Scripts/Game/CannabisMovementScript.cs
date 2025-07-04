using UnityEngine;

public class CannabisMovementScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float deadZone = -40f;
    public float simulationRate = 0.01f; // 100 Hz (10ms pro Schritt)
    private float simulationTimer = 0f;

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
