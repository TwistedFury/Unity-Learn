using UnityEngine;

public class Turret : MonoBehaviour
{

    [SerializeField] float rotation_speed = 90.0f;
    [SerializeField] float fire_rate = 1.0f;
    [SerializeField] Ammo ammo;
    [SerializeField] Transform barrel;

    float fire_timer = 0;

    void Start()
    {
        fire_timer = fire_rate;
    }

    void Update()
    {
        fire_timer -= Time.deltaTime;
        if (fire_timer <= 0)
        {
            Instantiate(ammo, barrel.position, barrel.rotation);
            fire_timer = fire_rate;
        }

        transform.Rotate(rotation_speed * Time.deltaTime * Vector3.up);
    }
}
