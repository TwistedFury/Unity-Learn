using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class DualSensePlayerLedHealth : MonoBehaviour
{
    // Plugged in via Type-C -> DualSenseGamepadHID should be present.
    private DualSenseGamepadHID _ds;

    // Call this when health changes (don't spam every frame unless you want to).
    public void SetHealth01(float health01)
    {
        // Re-acquire in case device hot-plugged / current changed
        if (_ds == null)
            _ds = InputSystem.devices.OfType<DualSenseGamepadHID>().FirstOrDefault();

        if (_ds == null)
            return; // no DualSense, nothing to control

        health01 = Mathf.Clamp01(health01);

        health01 = Mathf.Clamp01(health01);

        const int segments = 5;
        int lit = Mathf.CeilToInt(health01 * segments);
        byte mask = (byte)((1 << lit) - 1); // 0x00..0x1F (lights left->right)
        mask |= 0x20; // immediate
        SendPlayerLedMask((byte)(mask | 0x20)); // +0x20 = apply immediately (no fade) :contentReference[oaicite:3]{index=3}
    }
    void Awake()
    {
        // Grab the actual DualSense HID device (not whatever Gamepad.current happens to be)
        _ds = InputSystem.devices.OfType<DualSenseGamepadHID>().FirstOrDefault();
        Debug.Log(_ds != null
            ? $"DualSense found: {_ds.displayName} ({_ds.layout})"
            : "No DualSenseGamepadHID found (is Steam/DSX/DS4Windows virtualizing it?)");
    }

    private void SendPlayerLedMask(byte playerLedByte)
    {
        // DualSense USB output report is 48 bytes in this commonly-used layout. :contentReference[oaicite:4]{index=4}
        Span<byte> report = stackalloc byte[48];
        report.Clear();

        report[0] = 0x02;     // report id/type for USB :contentReference[oaicite:5]{index=5}
        report[1] = 0x10;     // flag: update player indicator LEDs :contentReference[oaicite:6]{index=6}
        report[44] = playerLedByte; // player LED bitmask :contentReference[oaicite:7]{index=7}

        var cmd = DualSenseHidOutputCommand.Create(report);
        _ds.ExecuteCommand(ref cmd);
    }

    // Minimal "HIDO" command wrapper (Unity Input System HID output). :contentReference[oaicite:8]{index=8}
    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    private unsafe struct DualSenseHidOutputCommand : IInputDeviceCommandInfo
    {
        public static FourCC Type => new FourCC('H', 'I', 'D', 'O');
        public FourCC typeStatic => Type;

        public const int ReportSize = 48;
        public const int kSize = InputDeviceCommand.BaseCommandSize + ReportSize;

        [FieldOffset(0)] public InputDeviceCommand baseCommand;
        [FieldOffset(InputDeviceCommand.BaseCommandSize)] public fixed byte report[ReportSize];

        public static DualSenseHidOutputCommand Create(ReadOnlySpan<byte> data)
        {
            var cmd = new DualSenseHidOutputCommand
            {
                baseCommand = new InputDeviceCommand(Type, kSize)
            };
            unsafe
            {
                byte* dst = cmd.report;
                for (int i = 0; i < ReportSize; i++)
                    dst[i] = i < data.Length ? data[i] : (byte)0;
            }

            return cmd;
        }
    }
}
