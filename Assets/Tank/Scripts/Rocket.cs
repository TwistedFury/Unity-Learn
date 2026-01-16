using UnityEngine;
using UnityEngine.InputSystem;

public class Rocket : Ammo
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
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null) health.OnDamage(damage);

        Debug.Log($"Hit {collision.gameObject.name}");
        Instantiate(effectOnDestroy, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
