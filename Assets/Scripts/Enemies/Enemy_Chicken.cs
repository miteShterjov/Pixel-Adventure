using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chicken : Enemy
{
    [Header("Chicken details")]
    [SerializeField] private float aggroDuration;
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float speedIncreaseRate = 0.5f;

    private float aggroTimer;
    private float lastTimeAttacked;
    private float currentMoveSpeed;
    private bool isChasing;
    private bool canFlip = true;

    protected override void Update()
    {
        base.Update();

        aggroTimer -= Time.deltaTime;

        if (isDead)
            return;

        bool canAttack = Time.time > lastTimeAttacked + attackCooldown;

        if (isPlayerDetected)
        {
            canMove = true;
            aggroTimer = aggroDuration;

            if (!isChasing && canAttack)
                Attack();

            if (isChasing)
            {
                // Gradually increase speed while chasing and player is in range
                currentMoveSpeed += speedIncreaseRate * Time.deltaTime;
            }
        }

        if (aggroTimer < 0)
        {
            canMove = false;
            isChasing = false;
            currentMoveSpeed = moveSpeed;
        }

        HandleMovement();

        if (isGrounded)
            HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!isGroundInfrontDetected || isWallDetected)
        {
            Flip();
            canMove = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Attack()
    {
        lastTimeAttacked = Time.time;
        isChasing = true;
        currentMoveSpeed = moveSpeed;
    }

    private void HandleMovement()
    {
        if (canMove == false) return;
        if (player == null) return;

        if (player != null) HandleFlip(player.transform.position.x);

        float speedToUse = isChasing ? currentMoveSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(speedToUse * facingDir, rb.linearVelocity.y);
    }

    protected override void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight)
        {
            if (canFlip)
            {
                canFlip = false;
                Invoke(nameof(Flip), .3f);
            }
        }
    }

    protected override void Flip()
    {
        base.Flip();
        canFlip = true;
    }
}
