using UnityEngine;
using UnityEngine.UI;

public class ReadyToFire : MonoBehaviour
{
    private Tank Tank;
    [SerializeField] Image image;

    [Header("States")]
    [SerializeField] Sprite notReady;
    [SerializeField] Sprite isReady;

    void Start()
    {
        Tank = FindFirstObjectByType<Tank>().GetComponent<Tank>();
    }

    void Update()
    {
        if (Tank.FireReady)
        {
            image.sprite = isReady;
        } else
        {
            if (image.sprite != notReady)
            {
                image.sprite = notReady;
            }
        }
    }
}
