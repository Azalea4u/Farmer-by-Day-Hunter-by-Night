using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

// Thank you https://www.youtube.com/watch?v=HCaSnZvs90g for the movement help, really appreciate it :D

public class Player : NetworkBehaviour
{
    [SerializeField] private int walkSpeed = 5;
    [SerializeField] private InputActionMap controls;

    private InputAction walkAction;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetupControls();

        controls.Enable();
    }

    private void FixedUpdate()
    {
        Vector3 walkValue = walkAction.ReadValue<Vector2>();

        walkValue.Normalize();  // Prevents faster diagonal movement lol

        rb.linearVelocity = walkValue * walkSpeed;
    }

    private void SetupControls()
    {
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
