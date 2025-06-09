using UnityEngine;

public class CannabisSpawnerScript : MonoBehaviour
{
    public GameObject cannabisPrefab;
    public float spawnRate = 1;
    private float timer = 0f;
    public float heightOffset = 11;

    void Start()
    {
        SpawnCannabis();
    }

    void Update()
    {
        Debug.Log("CannabisSpawner läuft, Timer: " + timer);
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SpawnCannabis();
            timer = 0;
        }

    }

    void SpawnCannabis()
    {
        float lowestPoint = transform.position.y;
        float highestPoint = transform.position.y + heightOffset;
        Instantiate(cannabisPrefab, new Vector3(transform.position.x + 10, Random.Range(lowestPoint, highestPoint)), transform.rotation);
        Debug.Log("Cannabis spawned at: " + transform.position.x + ", " + Random.Range(lowestPoint, highestPoint));
    }

}
