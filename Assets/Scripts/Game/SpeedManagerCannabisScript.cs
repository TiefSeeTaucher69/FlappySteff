using UnityEngine;

public class SpeedManagerCannabisScript : MonoBehaviour
{
    public static float currentSpeed = 7f;
    public static float acceleration = 0.1f;

    void Update()
    {
        currentSpeed += acceleration * Time.deltaTime;
    }
}

