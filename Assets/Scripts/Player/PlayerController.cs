using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Netcode;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Globalization;

// Thank you https://www.youtube.com/watch?v=HCaSnZvs90g for the movement help, really appreciate it :D

public class PlayerController : NetworkBehaviour
{
    [SerializeField] public Player player;

    [SerializeField] private int walkSpeed = 5;
    [SerializeField] private bool talkingToNPC = false;

    [SerializeField] private InputActionMap controls;

    [HideInInspector] public InputAction primaryAction;
    [HideInInspector] public InputAction secondaryAction;
    [HideInInspector] public InputAction walkAction;
    [HideInInspector] public InputAction menuAction;
    [HideInInspector] public InputAction dialogueAction;
    [HideInInspector] public InputAction mapAction;
    [HideInInspector] public InputAction swapAction;

    // Circle Collider is used for Dialogue Triggers & as a method of getting the Player's collision radius
    private float radius;
    private CircleCollider2D cc;

    private ContactFilter2D collisionFilter, dialogueFilter;
    private List<RaycastHit2D> currentCollisions;

    private void Start()
    {
        if (player == null) player = GetComponent<Player>();

        SetupControls();

        dialogueAction.performed += _ => Dialogue();

        controls.Enable();

        cc = GetComponent<CircleCollider2D>();
        radius = cc.radius;

        collisionFilter = new();
        collisionFilter = collisionFilter.NoFilter();
        collisionFilter.useTriggers = false;

        dialogueFilter = new();
        dialogueFilter = dialogueFilter.NoFilter();
        dialogueFilter.useTriggers = true;

        currentCollisions = new();
    }

    private void FixedUpdate()
    {
        Vector3 walkValue = walkAction.ReadValue<Vector2>();

        if (!talkingToNPC && (walkValue.x != 0 || walkValue.y != 0))
        {
            walkValue.Normalize();  // Prevents faster diagonal movement

            transform.position += CheckCollisions(walkValue, Time.deltaTime * walkSpeed);
        }
    }

    private Vector3 CheckCollisions(Vector3 moveDir, float distance)
    {
        if (Physics2D.CircleCast(transform.position, radius, moveDir, collisionFilter, currentCollisions, distance) > 0)
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

                    if (Physics2D.CircleCast(transform.position, radius, dirX, collisionFilter, results, distance) > 0)
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

                    if (Physics2D.CircleCast(transform.position, radius, dirY, collisionFilter, results, distance) > 0)
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
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
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

    // When within range of an NPC, press the DIALOGUE key (E) to INTERACT/TALK with them
    private void Dialogue()
    {
        if (!talkingToNPC && IsOwner)
        {
            List<RaycastHit2D> results = new();

            if (Physics2D.CircleCast(transform.position, radius, Vector2.zero, dialogueFilter, results) > 0)
            {
                foreach (RaycastHit2D hit in results)
                {
                    // Finds the first NPC in the collision results and attempts to TALK to them
                    if (hit.collider.isTrigger && hit.collider.gameObject.TryGetComponent<INPC>(out var npc))
                    {
                        talkingToNPC = true;
                        npc.TalkTo(player);
                        break;
                    }
                }
            }
        }
    }

    public void StopDialogue() { talkingToNPC = false; }
}
