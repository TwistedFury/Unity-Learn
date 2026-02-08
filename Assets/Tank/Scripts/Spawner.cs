using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed == true)
        {
            Vector3 pos = transform.position;
            pos.x += Random.Range(-5.0f, 5.0f);
            pos.z += Random.Range(-5.0f, 5.0f);
            _ = Instantiate(spawnObject, pos, transform.rotation);
        }
    }
}
