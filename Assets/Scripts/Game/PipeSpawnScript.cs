using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;

    public float baseSpawnRate = 3f;       // ursprüngliche Spawnrate bei Startgeschwindigkeit
    public float minSpawnRate = 0.5f;      // minimale Spawnrate, damit's nicht unspielbar wird
    public float startSpeed = 5f;          // Startgeschwindigkeit, muss mit SpeedManager übereinstimmen
    public float heightOffset = 10f;

    private float timer = 0f;

    void Start()
    {
        spawnPipe(); // Erste Pipe direkt spawnen
    }

    void Update()
    {
        float currentSpeed = SpeedManager.currentSpeed;

        // Spawnrate anpassen, aber Mindestgrenze einhalten
        float adjustedSpawnRate = Mathf.Clamp(baseSpawnRate * (startSpeed / currentSpeed), minSpawnRate, baseSpawnRate);

        if (timer < adjustedSpawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawnPipe();
            timer = 0f;
        }
    }

    void spawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        Instantiate(pipe,
                    new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint)),
                    transform.rotation);
    }
}
