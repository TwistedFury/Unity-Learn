using UnityEngine;
using UnityEngine.InputSystem;

public class Script : MonoBehaviour
{
    [SerializeField] float force = 1.0f;
    [SerializeField] GameObject effectOnDestroy;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * force, ForceMode.Impulse);
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Hit {collision.gameObject.name}");
        Destroy(gameObject);
        Instantiate(effectOnDestroy, transform.position, Quaternion.identity);
    }
}
