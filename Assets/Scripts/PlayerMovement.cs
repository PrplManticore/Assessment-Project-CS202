using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController heroController;
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] Transform cameraTransform;
    [SerializeField] GameObject basicAttack;
    [SerializeField] Transform basicTransform;
    [SerializeField] float attackRange = 30.0f;
    [SerializeField] Transform enemy;
    [SerializeField] LayerMask enemyLayer;

    InputActionMap actionMap;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    float currentVelocity;

    public Vector2 moveInput;

    // Sets the player's movement speed
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSmoothTime = 0.1f;
    [SerializeField] float jumpHeight = 0.5f;
    bool isGrounded;
    float verticalVelocity = -2.0f;
    float gravity = -9.81f;
    public float life = 3.0f;
    public float maxLife = 3.0f;

    [HideInInspector] public bool IsJumping;
    SlashScript damageDealt;

    private void Awake()
    {
        actionMap = inputActions.FindActionMap("Player");
        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");
        attackAction = actionMap.FindAction("Attack");
    }

    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }

    void Start()
    {
        heroController = GetComponent<CharacterController>();
    }

    public void EndGame()
    {
        // Standard command for built applications
        Application.Quit();

        // Exit play mode in the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void Update()
    {
        if (attackAction.WasPressedThisFrame())
        {
            Instantiate(basicAttack, basicTransform.position, basicTransform.rotation);
            if (damageDealt)
            {
                enemy.GetComponent<SkeletonScript>().life -= 1.0f;
            }
        }

        isGrounded = heroController.isGrounded;
        if(isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Small negative value to keep the player grounded
        }

        if(jumpAction.IsPressed() && isGrounded)
        {
            IsJumping = true;
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        else
        {
            IsJumping = false;
        }

        verticalVelocity += gravity * Time.deltaTime; // Apply gravity over time

        moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = Vector3.zero;
        if (moveInput.magnitude > 0.01f) // Check if there's significant input

        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);
            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        }
        Vector3 velocity = moveDir * speed + Vector3.up * verticalVelocity * gravity * -1;
        heroController.Move(velocity * Time.deltaTime);

        if (life == 0.0f)
        {
            Destroy(gameObject);
            EndGame();
        }
    }   
}