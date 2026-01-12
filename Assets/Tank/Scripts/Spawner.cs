using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    [SerializeField] float spawn_time = 1.0f;
    [SerializeField] GameObject spawn_object;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed == true)
        {
            Vector3 pos = transform.position;
            pos.x += Random.Range(-5.0f, 5.0f);
            pos.z += Random.Range(-5.0f, 5.0f);
            var go = Instantiate(spawn_object, pos, transform.rotation);
        }
    }
}
