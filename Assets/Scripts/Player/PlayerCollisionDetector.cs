using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [Space]
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;
    [SerializeField] private LayerMask whatIsEnemy;

    private bool isGrounded;
    private bool isWallDetected;
    private int facingDir = 1;

    public bool IsGrounded => isGrounded;
    public bool IsWallDetected => isWallDetected;
    public Transform EnemyCheck => enemyCheck;
    public float EnemyCheckRadius => enemyCheckRadius;
    public LayerMask WhatIsEnemy => whatIsEnemy;

    public void SetFacingDir(int dir)
    {
        facingDir = dir;
    }

    public void UpdateCollisions()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
    }
}
