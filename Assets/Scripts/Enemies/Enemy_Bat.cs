using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyBat : Enemy
    {
        [Header("Bat details")]
        [SerializeField] private float aggroRadius = 7;
        [SerializeField] private float chaseDuration = 1;
        [SerializeField] private float attackSpeed;

        private float defaultSpeed;
        private float chaseTimer;
        private Vector3 originalPosition;
        private Vector3 destination;
        private bool canDetectPlayer;
        private Collider2D target;
        
        private static readonly int MovingParam = Animator.StringToHash("isMoving");

        protected override void Awake()
        {
            base.Awake();

            defaultSpeed = moveSpeed;
            originalPosition = transform.position;
            canMove = false;
        }

        protected override void Update()
        {
            if (target && target.GetComponent<PlayerHealthController>().IsKnocked) return;
            
            base.Update();

            chaseTimer -= Time.deltaTime;

            if (idleTimer < 0 ) canDetectPlayer = true;

            HandleMovement();
            HandlePlayerDetection();
        }
        
        public override void Die()
        {
            rb.linearVelocity = Vector2.zero;
            base.Die();
            canMove = false;
        }

        protected override void HandleAnimator() {}

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawWireSphere(transform.position, aggroRadius);
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleMovement()
        {
            if (!canMove) return;

            HandleFlip(destination.x);
            transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            if (chaseTimer > 0 & target) destination = target.transform.position;
            else moveSpeed = attackSpeed;

            if (!(Vector2.Distance(transform.position, destination) < .1f)) return;
            if (destination == originalPosition)
            {
                idleTimer = idleDuration;
                canDetectPlayer = false;
                canMove = false;
                anim.SetBool(MovingParam, false);
                target = null;
                moveSpeed = defaultSpeed;
            }
            else destination = originalPosition;
        }

        private void HandlePlayerDetection()
        {
            if (target || !canDetectPlayer) return;
            target = Physics2D.OverlapCircle(transform.position, aggroRadius, whatIsPlayer);

            if (!target) return;
            chaseTimer = chaseDuration;
            destination = target.transform.position;
            canDetectPlayer = false;
            anim.SetBool(MovingParam, true);
        }

        private void AllowMovement() => canMove = true;

    }
}
