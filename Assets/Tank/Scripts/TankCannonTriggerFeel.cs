using System.Collections;
using UniSense;
using UnityEngine;

public class TankCannonTriggerFeel : MonoBehaviour
{
    [Header("UniSense Reference")]
    [SerializeField] DualSenseControls triggers;

    [Header("Ready-to-fire hard wall (stiff to a point)")]
    [Range(0f, 1f)] public float wallStart = 0.72f; // where stiffness begins
    [Range(0f, 1f)] public float wallEnd = 0.95f; // near full pull
    [Range(0f, 1f)] public float wallForce = 1.00f; // MAX stiffness

    [Header("Reload feel (still heavy, but not a wall)")]
    [Range(0f, 1f)] public float reloadStart = 0.15f;
    [Range(0f, 1f)] public float reloadForce = 0.55f;

    [Header("Impact kick on fire (the BOOM)")]
    [Range(0.02f, 0.25f)] public float kickDuration = 0.12f;
    [Range(0f, 1f)] public float kickBeginForce = 0.35f;
    [Range(0f, 1f)] public float kickMiddleForce = 1.00f;
    [Range(0f, 1f)] public float kickEndForce = 0.10f;
    [Range(0f, 1f)] public float kickFrequency = 1.00f;

    bool ready;
    Coroutine kickCo;

    void Awake()
    {
        if (triggers == null)
            triggers = GetComponent<DualSenseControls>();
    }

    void OnEnable()
    {
        ApplyReloadFeel();
    }

    void OnDisable()
    {
        if (kickCo != null) StopCoroutine(kickCo);
        kickCo = null;

        if (triggers != null)
            triggers.RightTriggerEffectType = (int)DualSenseTriggerEffectType.NoResistance;
    }

    /// <summary>
    /// Call every frame (or whenever it changes): true when the cannon is ready.
    /// </summary>
    public void SetReady(bool isReady)
    {
        if (ready == isReady) return;
        ready = isReady;

        if (kickCo != null) return; // don't swap modes mid-kick

        if (ready) ApplyHardWall();
        else ApplyReloadFeel();
    }

    /// <summary>
    /// Call exactly when the cannon fires.
    /// </summary>
    public void FireImpact()
    {
        if (triggers == null) return;

        if (kickCo != null) StopCoroutine(kickCo);
        kickCo = StartCoroutine(KickRoutine());
    }

    void ApplyHardWall()
    {
        if (triggers == null) return;

        // Stiff to a point: hard wall near the end of the pull
        triggers.RightTriggerEffectType = (int)DualSenseTriggerEffectType.SectionResistance;
        triggers.RightSectionStartPosition = wallStart;
        triggers.RightSectionEndPosition = wallEnd;
        triggers.RightSectionForce = wallForce;
    }

    void ApplyReloadFeel()
    {
        if (triggers == null) return;

        // Heavy mechanism while reloading
        triggers.RightTriggerEffectType = (int)DualSenseTriggerEffectType.ContinuousResistance;
        triggers.RightContinuousStartPosition = reloadStart;
        triggers.RightContinuousForce = reloadForce;
    }

    IEnumerator KickRoutine()
    {
        // The "impact": a short violent kick/shudder
        triggers.RightTriggerEffectType = (int)DualSenseTriggerEffectType.EffectEx;

        triggers.RightEffectStartPosition = 0.02f;
        triggers.RightEffectBeginForce = kickBeginForce;
        triggers.RightEffectMiddleForce = kickMiddleForce; // big hit
        triggers.RightEffectEndForce = kickEndForce;
        triggers.RightEffectFrequency = kickFrequency;   // max buzz = impact
        triggers.RightEffectKeepEffect = false;

        yield return new WaitForSeconds(kickDuration);

        kickCo = null;

        // After impact, return to reload feel (or wall if instantly ready)
        if (ready) ApplyHardWall();
        else ApplyReloadFeel();
    }
}
