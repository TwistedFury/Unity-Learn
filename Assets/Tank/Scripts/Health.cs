using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float max_health = 10;
    [SerializeField] GameObject hit_effect;
    [SerializeField] GameObject destroy_effect;

    [Header("Controller LEDs")]
    [SerializeField] DualSensePlayerLedHealth playerLeds;

    [SerializeField] Slider healthSlider;
    bool isPlayer;

    private float cur_health;

    public float Cur_Health
    {
        get { return cur_health; }
        set { cur_health = value; }
    }

    bool destroyed = false;

    void Awake()
    {
        // Optional: auto-find if you don’t want to drag it in
        if (gameObject.CompareTag("Player")) { isPlayer = true; }
        if (playerLeds == null && isPlayer) playerLeds = FindObjectOfType<DualSensePlayerLedHealth>(true);
    }

    void Start()
    {
        Cur_Health = max_health;
        UpdatePlayerLeds(); // set initial LED state (full health)
    }

    public void OnDamage(float damage)
    {
        if (destroyed) return;

        Cur_Health -= damage;
        if (isPlayer) UpdatePlayerLeds(); // update LEDs immediately on health change

        if (Cur_Health <= 0) destroyed = true;

        if (!destroyed && hit_effect != null)
            Instantiate(hit_effect, transform.position, Quaternion.identity);

        // Update Health Slider
        if (isPlayer)
        {
            healthSlider.value = cur_health / max_health;
        }

        if (destroyed)
        {
            if (destroy_effect != null)
                Instantiate(destroy_effect, transform.position, Quaternion.identity);
            if (isPlayer) { TankGameManager.Instance.GameLoss(); return; }
            Destroy(gameObject);
        }
    }

    void UpdatePlayerLeds()
    {
        if (playerLeds == null) return;

        float health01 = Mathf.Clamp01(Cur_Health / max_health);
        playerLeds.SetHealth01(health01);
    }
}
