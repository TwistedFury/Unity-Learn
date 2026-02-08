using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class CarController : MonoBehaviour
{
    [Serializable]
    public struct Wheel
    {
        public WheelCollider collider;
        public Transform transform;
    }
    // axle contains the left and right wheel, can be used to steer or motor
    [Serializable]
    public struct Axle
    {
        public Wheel leftWheel;
        public Wheel rightWheel;
        public bool isMotor;
        public bool isSteering;
    }
    [SerializeField] Axle[] axles;
    [SerializeField, Range(0, 1000)] float maxMotorTorque = 400.0f;
    [SerializeField, Range(0, 90)] float maxSteeringAngle = 30.0f;
    InputAction moveAction;
    Vector2 inputMovement = Vector2.zero;
    void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }
    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
    }
    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
    }
    public void FixedUpdate()
    {
        foreach (Axle axle in axles)
        {
            if (axle.isSteering)
            {
                axle.leftWheel.collider.steerAngle = inputMovement.x *
                maxSteeringAngle;
                axle.rightWheel.collider.steerAngle = inputMovement.x *
                maxSteeringAngle;
            }
            if (axle.isMotor)
            {
                axle.leftWheel.collider.motorTorque = inputMovement.y *
                maxMotorTorque;
                axle.rightWheel.collider.motorTorque = inputMovement.y *
                maxMotorTorque;
            }
            // update wheel transform
            UpdateWheelTransform(axle.leftWheel);
            UpdateWheelTransform(axle.rightWheel);
        }
    }
    public void UpdateWheelTransform(Wheel wheel)
    {
        if (wheel.transform == null) return;
        // update the position and rotation of the wheel
        wheel.collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheel.transform.position = position;
        wheel.transform.rotation = rotation;
    }
    private void OnMove(InputAction.CallbackContext ctx)
    {
        inputMovement = ctx.ReadValue<Vector2>();
    }
}