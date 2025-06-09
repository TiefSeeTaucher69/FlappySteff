using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static float currentSpeed { get; private set; } = 5f;
    public static float acceleration = 0.1f;
    public static float maxSpeed = 15f; // Beispielwert, passt du nach Bedarf an

    void Awake()
    {
        // Sicherstellen, dass es nur eine Instanz gibt
        if (FindObjectsOfType<SpeedManager>().Length > 1)
        {
            Debug.LogWarning("Mehrere SpeedManager vorhanden – das sollte nicht passieren!");
        }
    }

    void Update()
    {
        currentSpeed += acceleration * Time.deltaTime;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    }
}
