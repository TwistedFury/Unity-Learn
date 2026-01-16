using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [Header("Lifetime in seconds")]
    [SerializeField] float lifetime;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
