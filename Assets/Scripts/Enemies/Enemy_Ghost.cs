using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Ghost : Enemy
{
    [Header("Ghost details"), Space]
    [SerializeField] private float agrroRadius;
    [SerializeField] private float activeDuration;
    [SerializeField] private float activeTimer;
    [Space]
    [SerializeField] private float xMinDistance;
    [SerializeField] private float yMinDistance;
    [SerializeField] private float yMaxDistance;
    [SerializeField] private Transform target;

    private bool isChasing;

    protected override void Update()
    {
        base.Update();

        if (isDead) return;
        if (target == null)
        {
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, agrroRadius, whatIsPlayer);
            if (playerCollider != null) target = playerCollider.gameObject.transform;
            return;
        }

        activeTimer -= Time.deltaTime;


        if (isChasing == false && idleTimer < 0 && target != null) StartChase();
        else if(isChasing && activeTimer < 0) EndChase();
        
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (canMove == false) return;

        HandleFlip(target.position.x);
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void StartChase()
    {
        if (target == null)
        {
            EndChase();
            return;
        }

        float xOffset = UnityEngine.Random.Range(0, 100) < 50 ? -1 : 1;
        float yPosition = UnityEngine.Random.Range(yMinDistance, yMaxDistance);

        transform.position = target.position + new Vector3(xMinDistance * xOffset, yPosition);

        activeTimer = activeDuration;
        isChasing = true;
        anim.SetTrigger("appear");
    }

    private void EndChase()
    {
        idleTimer = idleDuration;
        isChasing = false;
        anim.SetTrigger("desappear");
    }

    private void MakeInvisible()
    {
        sr.color = Color.clear;
        EnableColliders(false);
    }
    private void MakeVisible()
    {
        sr.color = Color.white;
        EnableColliders(true);
    }

    public override void Die()
    {
        float dieFallSpeed = -5;
        rb.linearVelocity = Vector2.zero;
        base.Die();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, dieFallSpeed);
        canMove = false;
    }
    protected override void HandleAnimator()
    {
        
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, agrroRadius);
    }
}
