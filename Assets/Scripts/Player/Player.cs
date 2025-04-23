using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

// Thank you https://www.youtube.com/watch?v=HCaSnZvs90g for the movement help, really appreciate it :D

public class Player : MonoBehaviour
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

    private float radius;
    private CircleCollider2D cc;
    private ContactFilter2D contactFilter;
    private List<RaycastHit2D> currentCollisions;

    private void Start()
    {
        SetupControls();

        controls.Enable();

        cc = GetComponent<CircleCollider2D>();
        radius = cc.radius;

        contactFilter = new();
        contactFilter = contactFilter.NoFilter();
        contactFilter.useTriggers = false;

        currentCollisions = new();
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
        if (Physics2D.CircleCast(transform.position, radius, moveDir, contactFilter, currentCollisions, distance) > 0)
        {
            Vector3 resolvedMoveDir = moveDir;
            Vector3 manipulatedMoveDir;

            float distanceX = distance;
            float distanceY = distance;

            List<RaycastHit2D> results = new();

            foreach (RaycastHit2D hit in currentCollisions)
            {
                if (hit.collider != cc)
                {
                    manipulatedMoveDir = resolvedMoveDir;

                    Vector2 dirX = Vector2.zero;
                    dirX.x = manipulatedMoveDir.x;

                    if (Physics2D.CircleCast(transform.position, radius, dirX, contactFilter, results, distance) > 0)
                    {
                        foreach (RaycastHit2D hitX in results)
                        {
                            if (hitX.collider != cc)
                            {
                                float tempDistanceX = Mathf.Abs(Vector2.Distance(hitX.point, transform.position)) - radius;
                                if (tempDistanceX < distanceX) distanceX = tempDistanceX;
                            }
                        }
                    }

                    Vector2 dirY = Vector2.zero;
                    dirY.y = manipulatedMoveDir.y;

                    if (Physics2D.CircleCast(transform.position, radius, dirY, contactFilter, results, distance) > 0)
                    {
                        foreach (RaycastHit2D hitY in results)
                        {
                            if (hitY.collider != cc)
                            {
                                float tempDistanceY = Mathf.Abs(Vector2.Distance(hitY.point, transform.position)) - radius;
                                if (tempDistanceY < distanceY) distanceY = tempDistanceY;
                            }
                        }
                    }

                    manipulatedMoveDir.x *= distanceX;
                    manipulatedMoveDir.y *= distanceY;
                }
                else continue;

                if (Mathf.Abs(manipulatedMoveDir.x) < Mathf.Abs(resolvedMoveDir.x)) resolvedMoveDir.x = manipulatedMoveDir.x;
                if (Mathf.Abs(manipulatedMoveDir.y) < Mathf.Abs(resolvedMoveDir.y)) resolvedMoveDir.y = manipulatedMoveDir.y;
            }

            if (resolvedMoveDir != moveDir) return resolvedMoveDir;
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
