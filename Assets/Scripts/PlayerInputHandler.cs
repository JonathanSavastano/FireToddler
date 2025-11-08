using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour 
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string shoot = "Shoot";
    [SerializeField] private string interact = "Interact";

    private InputAction  movementAction;
    private InputAction  rotationAction;
    private InputAction  jumpAction;
    private InputAction  sprintAction;
    private InputAction  shootAction;
    private InputAction  interactAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }

    [Header("Water Particle Settings")]
    public ParticleSystem waterParticles;
    public float spawnDistance = 0.5f;

    [Header("Water Ammo Settings")]
    public float maxWaterTime = 10f;
    private float currentWaterTime;
    private bool isShooting = false;
    private bool isOutOfWater = false;


    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        shootAction = mapReference.FindAction(shoot);
        interactAction = mapReference.FindAction(interact);

        SubscribeActionValuesToInputEvents();

        currentWaterTime = maxWaterTime;
    }

    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;

        shootAction.performed += _ => { Debug.Log("Shoot pressed"); TryStartWater(); }; 
        shootAction.canceled += _ => { Debug.Log("Shoot released"); StopWater(); };

        interactAction.performed += _ => { Debug.Log("Interact pressed"); RefillWater(); };
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
        if (interactAction != null)
        {
            interactAction.Enable();
        }
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
        if (interactAction != null)
        {
            interactAction.Disable();
        }
    }

    private void Update()
    {
        if (isShooting && !isOutOfWater)
        {
            currentWaterTime -= Time.deltaTime;

            if (currentWaterTime <= 0f)
            {
                currentWaterTime = 0f;
                isOutOfWater = true;
                StopWater();
                Debug.Log("Out of water!");
            }
        }
    }

    private void TryStartWater()
    {
        if (isOutOfWater)
        {
            Debug.Log("No water left to shoot!");
            return;
        }

        StartWater();
    }

    private void StartWater()
    {
        if (waterParticles == null) return;

        var emission = waterParticles.emission;
        emission.enabled = true;

        if (!waterParticles.isPlaying)
            waterParticles.Play();

        isShooting = true;
    }

    private void StopWater()
    {
        if (waterParticles == null) return;

        var emission = waterParticles.emission;
        emission.enabled = false;

        isShooting = false;
    }

    public void RefillWater()
    {
        currentWaterTime = maxWaterTime;
        isOutOfWater = false;
        isShooting = false;

        if (waterParticles != null)
        {
            var emission = waterParticles.emission;
            emission.enabled = false;
            waterParticles.Stop();
        }

        Debug.Log("Water refilled!");
    }
}
