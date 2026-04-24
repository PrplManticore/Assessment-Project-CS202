using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    InputActionMap actionMap;
    InputAction lookAction;

    [SerializeField] Transform playerTransform;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float cameraHeight = 1.5f;
    [SerializeField] Vector2 pitchMinMax = new Vector2(-30, 80);
    [SerializeField] float cameraSensitivity = 1.0f;

    float yaw, pitch;

    private void Awake()
    {
        actionMap = inputActions.FindActionMap("Player");
        lookAction = actionMap.FindAction("Look");
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseInput = lookAction.ReadValue<Vector2>();
        yaw += mouseInput.x * cameraSensitivity;
        pitch -= mouseInput.y * cameraSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        Quaternion desiredRotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offSet = desiredRotation * new Vector3(0, 0, -3);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 10.0f * Time.deltaTime);

        Vector3 desiredPosition = playerTransform.position + Vector3.up * cameraHeight + offSet;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 10.0f * Time.deltaTime);
    }
}
