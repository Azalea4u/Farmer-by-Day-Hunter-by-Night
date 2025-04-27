using UnityEngine;
using UnityEngine.InputSystem;

// Thank you https://www.youtube.com/watch?v=HCaSnZvs90g for the movement help, really appreciate it :D

public class Player : MonoBehaviour
{
    [SerializeField] private int walkSpeed = 5;

    [Header("Input Actions")]
    [SerializeField] private InputActionMap controls;

    private InputAction primaryAction;
    private InputAction secondaryAction;
    private InputAction walkAction;
    private InputAction menuAction;
    private InputAction dialogueAction;
    private InputAction mapAction;
    private InputAction swapAction;

    private float radius;
    private CircleCollider2D cc;

    private void Start()
    {
        SetupControls();

        controls.Enable();

        cc = GetComponent<CircleCollider2D>();
        radius = cc.radius;
    }

    private void FixedUpdate()
    {
        Vector3 walkValue = walkAction.ReadValue<Vector2>();

        if (walkValue.x != 0 || walkValue.y != 0)
        {
            walkValue.Normalize();  // Prevents faster diagonal movement

            transform.position += CheckCollisions(walkValue, Time.deltaTime * walkSpeed);
        }
    }

    private Vector3 CheckCollisions(Vector3 moveDir, float distance)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, moveDir, distance);

        if (hit && hit.collider != cc)
        {
            Vector3 resolvedMoveDir = moveDir;

            float distanceX = distance;
            float distanceY = distance;

            Vector2 dirX = Vector2.zero;
            dirX.x = moveDir.x;

            RaycastHit2D hitX = Physics2D.CircleCast(transform.position, radius, dirX, distance);
            if (hitX && hitX.collider != cc) { distanceX = Mathf.Abs(Vector2.Distance(hitX.point, transform.position)) - radius; }

            Vector2 dirY = Vector2.zero;
            dirY.y = moveDir.y;

            RaycastHit2D hitY = Physics2D.CircleCast(transform.position, radius, dirY, distance);
            if (hitY && hitY.collider != cc) { distanceY = Mathf.Abs(Vector2.Distance(hitY.point, transform.position)) - radius; }

            resolvedMoveDir.x *= distanceX;
            resolvedMoveDir.y *= distanceY;

            return resolvedMoveDir;
        }

        return moveDir * distance;
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
}
