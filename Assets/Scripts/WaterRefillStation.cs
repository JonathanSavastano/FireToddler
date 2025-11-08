using UnityEngine;
using UnityEngine.InputSystem;

public class WaterRefillStation : MonoBehaviour
{
    private InputAction interactAction;
    private bool playerInRange = false;
    private PlayerInputHandler playerHandler;

    private PlayerInput playerInput;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerHandler = other.GetComponent<PlayerInputHandler>();
            playerInput = other.GetComponent<PlayerInput>(); 

            if (playerInput != null && playerInput.actions != null)
            {
                interactAction = playerInput.actions.FindAction("Interact");
                if (interactAction != null)
                {
                    interactAction.performed += OnInteract;
                }
                else
                {
                    Debug.LogWarning("Interact action not found in PlayerInput actions.");
                }
            }

            Debug.Log("Press E to refill water");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactAction != null)
            {
                interactAction.performed -= OnInteract;
                interactAction = null;
            }

            playerHandler = null;
            playerInput = null;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!playerInRange || playerHandler == null)
            return;

        playerHandler.RefillWater();
        Debug.Log("Water tank refilled!");
    }
}
