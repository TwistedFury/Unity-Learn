using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyTest : MonoBehaviour
{
    [SerializeField] Vector3 force;
    [SerializeField] Vector3 torque;
    [SerializeField] ForceMode forceMode;
    [SerializeField, Range(0, 10)] float mass = 1.0f;
    [SerializeField] bool useGravity = true;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.mass = mass;
        rb.useGravity = useGravity;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            rb.AddForce(force, forceMode);
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            rb.AddTorque(force, forceMode);
        }
    }
}
