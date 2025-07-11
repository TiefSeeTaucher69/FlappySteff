using UnityEngine;

public class LaserShot : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 4f;
    public GameObject hitEffectPrefab;

    void Start()
    {
        Destroy(gameObject, lifetime); // Nach X Sekunden zerstören
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pipe"))
        {
            // Pipe zerstören
            Destroy(other.gameObject);

            // Hitmarker erzeugen an dieser Position
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            // Laser selbst zerstören
            Destroy(gameObject);
        }
    }
}
