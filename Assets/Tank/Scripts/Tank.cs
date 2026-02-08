using DualSenseSample.Inputs;
using UniSense;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tank : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float rotationSpeed = 90.0f;

    [Header("Shooting")]
    [SerializeField] GameObject ammo;
    [SerializeField] GameObject barrel;

    [SerializeField, Range(0.1f, 2.0f)] float fire_rate = 1.0f;
    float fire_timer;

    [Header("UniSense Components (drag these if you want)")]
    [SerializeField] DualSenseControls triggerControl;
    [SerializeField] DualSenseTouchpadColor colorControl;
    [SerializeField] TankCannonTriggerFeel triggerFeel;

    [Header("Audio")]
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip reloadSound;

    AudioSource fire, reload;

    InputAction moveAction;
    InputAction attackAction;

    private bool fireReady = false;
    public bool FireReady { get { return fireReady; } }
    private bool reloaded = false;

    void Awake()
    {
        // Prefer explicit inspector refs, otherwise try to auto-find.
        if (triggerControl == null) triggerControl = GetComponent<DualSenseControls>();
        if (colorControl == null) colorControl = GetComponent<DualSenseTouchpadColor>();
        if (triggerFeel == null) triggerFeel = GetComponent<TankCannonTriggerFeel>();

        // Initialize Audio
        if (fireSound != null) { fire = gameObject.AddComponent<AudioSource>(); fire.clip = fireSound; }
        if (reloadSound != null) { reload = gameObject.AddComponent<AudioSource>(); reload.clip = reloadSound; }
    }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        if (attackAction != null)
            attackAction.performed += _ => OnAttack();

        fire_timer = fire_rate;

        if (triggerControl == null)
            Debug.LogError("[Tank] Missing DualSenseControls (UniSense). Add it to the Tank object (or assign in Inspector).");

        if (colorControl == null)
            Debug.LogError("[Tank] Missing DualSenseTouchpadColor (UniSense). Add it to the Tank object (or assign in Inspector).");

        // Don’t silently continue if UniSense output components are missing.
        if (triggerControl == null || colorControl == null)
            return;

        SetColor(new Color32(255, 0, 135, 255)); // pink
    }

    void SetColor(Color32 c)
    {
        colorControl.LightBarColor = (Color)c;
    }

    void Update()
    {
        if (moveAction != null)
        {
            float direction = moveAction.ReadValue<Vector2>().y;
            transform.Translate(Vector3.forward * direction * moveSpeed * Time.deltaTime);

            float rotation = moveAction.ReadValue<Vector2>().x;
            transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);
        }

        fire_timer -= Time.deltaTime;

        if (fire_timer < reloadSound.length && !reload.isPlaying && !reloaded) { reload.Play(); reloaded = true; }

        if (fire_timer <= 0f && colorControl != null)
        {
            SetColor(new Color32(0, 255, 0, 255));
            fireReady = true;
        }

        triggerFeel?.SetReady(fire_timer <= 0f);
    }

    void OnAttack()
    {
        if (fire_timer > 0f) return;

        Instantiate(ammo, barrel.transform.position, barrel.transform.rotation);
        triggerFeel?.FireImpact();

        fire_timer = fire_rate;
        fireReady = false;

        if (colorControl != null)
            SetColor(new Color32(255, 0, 135, 255));
        fire.Play();
        reloaded = false;
    }
}
