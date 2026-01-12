using UnityEngine;
using UnityEngine.InputSystem;

public class Tank : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float rotationSpeed = 90.0f; // rotation in degrees per second

    [SerializeField] GameObject ammo;
    [SerializeField] GameObject barrel;

    void Start()
    {
        //
    }

    void Update()
    {
        // direction (forward/backward movement)
        float direction = 0.0f;
        if (Keyboard.current.wKey.isPressed) direction = +1.0f;
        if (Keyboard.current.sKey.isPressed) direction = -1.0f;

        // translate (move) the tank in the forward direction
        // moves the tank in the relative direction (direction tank is facing)
        transform.Translate(Vector3.forward * direction * moveSpeed * Time.deltaTime);

        // rotation (left/right)
        float rotation = 0.0f;
        if (Keyboard.current.aKey.isPressed) rotation = -1.0f;
        if (Keyboard.current.dKey.isPressed) rotation = +1.0f;

        // rotate the tank, around the up axis (y-axis)
        transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);

        // check if "Fire" key is pressed, if so instantiate the ammo (rocket)
        // ammo is instantiate at the muzzle position and rotation
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Instantiate(ammo, barrel.transform.position, barrel.transform.rotation);
        }
    }
}
