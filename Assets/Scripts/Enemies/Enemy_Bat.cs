using System;
using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Header("Bat details"), Space]
    [SerializeField] private float agrroRadius = 7;
    [SerializeField] private float chaseDuration = 1;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float dieFallSpeed = -5;

    private float defaultSpeed;
    private float chaseTimer;

    private Vector3 originalPosition;
    private Vector3 destination;

    private bool canDetectPlayer;
    private Collider2D target;

    protected override void Awake()
    {
        base.Awake();

        defaultSpeed = moveSpeed;
        originalPosition = transform.position;
        canMove = false;
    }

    protected override void Update()
    {
        if (target != null && target.GetComponent<PlayerHealthController>().IsKnocked) return;
        base.Update();

        chaseTimer -= Time.deltaTime;

        if (idleTimer < 0 )
            canDetectPlayer = true;

        HandleMovement();
        HandlePlayerDetection();
    }

    private void HandleMovement()
    {
        if (canMove == false)
            return;

        HandleFlip(destination.x);
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (chaseTimer > 0 & target != null)
            destination = target.transform.position;
        else
            moveSpeed = attackSpeed;

        if (Vector2.Distance(transform.position, destination) < .1f)
        {
            if (destination == originalPosition)
            {
                idleTimer = idleDuration;
                canDetectPlayer = false;
                canMove = false;
                anim.SetBool("isMoving", false);
                target = null;
                moveSpeed = defaultSpeed;
            }
            else
            {
                destination = originalPosition;
            }
        }
    }

    private void HandlePlayerDetection()
    {
        if (target == null && canDetectPlayer)
        {
            target = Physics2D.OverlapCircle(transform.position, agrroRadius, whatIsPlayer);

            if (target != null)
            {
                chaseTimer = chaseDuration;
                destination = target.transform.position;
                canDetectPlayer = false;
                anim.SetBool("isMoving", true);
            }
        }
    }

    private void AllowMovement() => canMove = true;

    protected override void HandleAnimator()
    {
        
    }

    public override void Die()
    {
        rb.linearVelocity = Vector2.zero;
        
        // Call base but then override the upward velocity with downward
        base.Die();
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, dieFallSpeed);
        canMove = false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, agrroRadius);
    }
}
