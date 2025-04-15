using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

// Thank you https://www.youtube.com/watch?v=HCaSnZvs90g for the movement help, really appreciate it :D

public class Player : NetworkBehaviour
{
    [SerializeField] private int walkSpeed = 5;
    [SerializeField] private InputActionMap controls;

    private InputAction primaryAction;
    private InputAction secondaryAction;
    private InputAction walkAction;
    private InputAction menuAction;
    private InputAction dialogueAction;
    private InputAction mapAction;
    private InputAction swapAction;

    private void Start()
    {
        SetupControls();

        controls.Enable();
    }

    private void FixedUpdate()
    {
        Vector3 walkValue = walkAction.ReadValue<Vector2>();

        walkValue.Normalize();  // Prevents faster diagonal movement

        transform.position += Time.deltaTime * walkSpeed * walkValue;
    }

    private void SetupControls()
    {
        primaryAction = controls.FindAction("Primary");
        if (primaryAction == null)
        {
            primaryAction = controls.AddAction("Primary");
            primaryAction.AddBinding("<Mouse>/leftButton");
        }

        secondaryAction = controls.FindAction("Secondary");
        if (secondaryAction == null)
        {
            secondaryAction = controls.AddAction("Secondary");
            secondaryAction.AddBinding("<Mouse>/rightButton");
        }

        walkAction = controls.FindAction("Walk");
        if (walkAction == null)
        {
            walkAction = controls.AddAction("Walk");
            walkAction.AddCompositeBinding("2DVector")
                .With("Up",    "<Keyboard>/w")
                .With("Down",  "<Keyboard>/s")
                .With("Left",  "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
        }

        menuAction = controls.FindAction("Menu");
        if (menuAction == null)
        {
            menuAction = controls.AddAction("Menu");
            menuAction.AddBinding("<Keyboard>/escape");
            menuAction.AddBinding("<Keyboard>/tab");
        }

        dialogueAction = controls.FindAction("Dialogue");
        if (dialogueAction == null)
        {
            dialogueAction = controls.AddAction("Dialogue");
            dialogueAction.AddBinding("<Keyboard>/e");
        }

        mapAction = controls.FindAction("Map");
        if (mapAction == null)
        {
            mapAction = controls.AddAction("Map");
            mapAction.AddBinding("<Keyboard>/m");
        }

        swapAction = controls.FindAction("Swap");
        if (swapAction == null)
        {
            swapAction = controls.AddAction("Swap");
            swapAction.AddBinding("<Keyboard>/q");
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }
}
