using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RollerPlayer : MonoBehaviour
{
    [SerializeField, Range(0, 50)] float moveForce = 3;
    [SerializeField, Range(0, 50)] float jumpForce = 3;
    [SerializeField] Transform view;

    [Header("Ground Collision")]
    [SerializeField, Range(0, 5)] float rayLength = 1;
    [SerializeField] LayerMask groundLayerMask = Physics.AllLayers;

    Vector2 moveInput;
    Rigidbody rb;

    InputAction moveAction;
    InputAction jumpAction;

    void Awake()
    {
        // If view is null, get the main camera
        view ??= Camera.main.transform;

        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        jumpAction.started += OnJump;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        jumpAction.started -= OnJump;
    }

    // Guarranteed to run 50 times per frame.
    private void FixedUpdate()
    {
        // Convert controller space to world space
        Vector3 movement = new(moveInput.x, 0, moveInput.y);
        // Convert world space to camera space / view space
        movement = Quaternion.AngleAxis(view.rotation.eulerAngles.y, Vector3.up) * movement;
        rb.AddForce(movement * moveForce, ForceMode.Force);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.beige);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (OnGround()) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool OnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayerMask);
    }
}
