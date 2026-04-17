using UnityEngine;

namespace Enemies
{
    public class EnemyGhost : Enemy
    {
        [Header("Ghost details"), Space]
        [SerializeField] private float aggroRadius;
        [SerializeField] private float activeDuration;
        [SerializeField] private float activeTimer;
        [Space]
        [SerializeField] private float xMinDistance;
        [SerializeField] private float yMinDistance;
        [SerializeField] private float yMaxDistance;
        [SerializeField] private Transform target;

        private bool isChasing;
        
        private static readonly int AppearParam = Animator.StringToHash("appear");
        private static readonly int DisappearParam = Animator.StringToHash("disappear");

        protected override void Update()
        {
            base.Update();

            if (isDead) return;
            if (!target)
            {
                Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, aggroRadius, whatIsPlayer);
                if (playerCollider) target = playerCollider.gameObject.transform;
                return;
            }

            activeTimer -= Time.deltaTime;

            if (!isChasing && idleTimer < 0 && target) StartChase();
            else if(isChasing && activeTimer < 0) EndChase();
        
            HandleMovement();
        }
        
        public override void Die()
        {
            const float dieFallSpeed = -5;
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

            Gizmos.DrawWireSphere(transform.position, aggroRadius);
        }

        private void HandleMovement()
        {
            if (!canMove) return;

            HandleFlip(target.position.x);
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        private void StartChase()
        {
            if (!target)
            {
                EndChase();
                return;
            }

            float xOffset = Random.Range(0, 100) < 50 ? -1 : 1;
            float yPosition = Random.Range(yMinDistance, yMaxDistance);

            transform.position = target.position + new Vector3(xMinDistance * xOffset, yPosition);

            activeTimer = activeDuration;
            isChasing = true;
            anim.SetTrigger(AppearParam);
        }

        private void EndChase()
        {
            idleTimer = idleDuration;
            isChasing = false;
            anim.SetTrigger(DisappearParam);
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
    }
}
