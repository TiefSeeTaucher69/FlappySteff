using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float destroyTime = 0.5f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
