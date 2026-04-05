using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(Rigidbody))]
public class DroneMovement : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerSettingsSO data;
    [SerializeField] private Transform droneVisual;
    [SerializeField] private GameObject cameraThirdPerson;
    [SerializeField] private GameObject cameraFirstPerson;


    private Rigidbody rb;
    private HealthSystem healthSystem;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalInput;
    private bool isThirdPerson = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
        ClampVelocity();
        ApplyVisualTilt();
    }

    private void OnCollisionEnter(Collision other)
    {
        healthSystem.DoDamage(other.relativeVelocity.magnitude * data.MultiplyDamageCollision);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
    private void OnUpDown(InputValue value)
    {
        verticalInput = value.Get<float>();
    }
    private void OnSwitchCamera(InputValue value)
    {
        if (value.isPressed)
        {
            isThirdPerson = !isThirdPerson;
            cameraFirstPerson.SetActive(!isThirdPerson);
            cameraThirdPerson.SetActive(isThirdPerson);
        }
    }

    private void ApplyMovement()
    {
        Vector3 localDirection = new(moveInput.x, verticalInput, moveInput.y);
        Vector3 worldDirection = transform.TransformDirection(localDirection);
        rb.AddForce(worldDirection * data.Force);
        rb.AddForce(data.VerticalForce * verticalInput * Vector3.up);
    }

    private void ApplyRotation()
    {
        float angle = lookInput.x * data.RotationSpeedX * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up, angle, Space.World);
    }

    private void ClampVelocity()
    {
        Vector3 horizontal = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float vertical = rb.linearVelocity.y;

        horizontal = Vector3.ClampMagnitude(horizontal, data.MaxHorizontalSpeed);
        vertical = Mathf.Clamp(vertical, -data.MaxVerticalSpeed, data.MaxVerticalSpeed);

        rb.linearVelocity = new Vector3(horizontal.x, vertical, horizontal.z);
    }

    private void ApplyVisualTilt()
    {
        float targetPitch = moveInput.y <= 0 ? (moveInput.y * data.TiltAngle * 0.1f) : (moveInput.y * data.TiltAngle);
        float targetRoll = -moveInput.x * data.TiltAngle;

        Quaternion playerRotation = Quaternion.Euler(targetPitch, 0f, targetRoll);

        droneVisual.localRotation = Quaternion.Lerp(
            droneVisual.localRotation,
            playerRotation,
            Time.fixedDeltaTime * data.TiltSpeed
            );
    }
}