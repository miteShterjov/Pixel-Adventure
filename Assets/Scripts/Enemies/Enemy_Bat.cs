using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Header("Bat Settings"), Space]
    [SerializeField] private float aggroRange = 10;
    [SerializeField] private bool canBeAgressive;
    
    public Collider2D playerAsTarget;

    private Vector3 originalPosition;
    private Vector3 destination;

    protected override void Awake()
    {
        base.Awake();

        originalPosition = transform.position;
        canMove = false;
    }

    protected override void Update()
    {
        base.Update();
        
    }
    
    protected override void HandleAnimator()
    {
        base.HandleAnimator();

        HandlePlayerDetection();
    }

    private void HandlePlayerDetection()
    {
        if (canBeAgressive == false) return;
        if (playerAsTarget == null) return;

        playerAsTarget = Physics2D.OverlapCircle(transform.position, aggroRange, whatIsPlayer);
        
        float distanceToPlayer = Vector2.Distance(transform.position, playerAsTarget.transform.position);

        if (distanceToPlayer <= aggroRange)
        {
            destination = playerAsTarget.transform.position;
            canMove = true;
        }
        else
        {
            destination = originalPosition;
            canMove = false;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
