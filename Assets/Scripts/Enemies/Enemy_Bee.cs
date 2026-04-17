using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyBee : Enemy
    {
        [Header("Bee details")]
        [SerializeField] private EnemyBulletBee bulletPrefab;
        [SerializeField] private Transform gunPoint;
        [SerializeField] private float bulletSpeed = 7;
        [SerializeField] private float bulletLifeTime = 2.5f;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float offset = .25f;

        private float lastTimeAttacked;
        private readonly List<Vector3> wayPoints = new List<Vector3>();
        private int wayIndex;
        private static readonly int AttackParam = Animator.StringToHash("attack");

        private Transform target;

        protected override void Start()
        {
            base.Start();
            canMove = false;
            CreateWayPoints();

            float randomValue = Random.Range(0, .6f);
            Invoke(nameof(AllowMovement), randomValue);
        }

        private void CreateWayPoints()
        {
            wayPoints.Add(transform.position + new Vector3(offset, offset));
            wayPoints.Add(transform.position + new Vector3(offset, -offset));
            wayPoints.Add(transform.position + new Vector3(-offset, -offset));
            wayPoints.Add(transform.position + new Vector3(-offset, offset));
        }

        protected override void Update()
        {
            base.Update();

            HandleMovement();
            FindTargetIfEmpty();

            bool canAttack = Time.time > lastTimeAttacked + attackCooldown && target;

            if (canAttack) Attack();
        }

        public override void Die()
        {
            rb.linearVelocity = Vector2.zero;
        
            base.Die();
            canMove = false;
        }
        
        protected override void HandleAnimator() { }
        
        private void FindTargetIfEmpty()
        {
            if (target) return;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, float.MaxValue, whatIsPlayer);

            if (hit.transform) target = hit.transform;
        }

        private void HandleMovement()
        {
            if (!canMove) return;
            if (isDead) return;

            transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayIndex], moveSpeed * Time.deltaTime);

            if (!(Vector2.Distance(transform.position, wayPoints[wayIndex]) < .1f)) return;
            wayIndex++;

            if (wayIndex >= wayPoints.Count) wayIndex = 0;
        }

        private void Attack()
        {
            lastTimeAttacked = Time.time;
            anim.SetTrigger(AttackParam);
        }

        private void CreateBullet()
        {
            EnemyBulletBee newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
            newBullet.SetupBullet(target, bulletSpeed, bulletLifeTime);

            target = null;
        }
       
        private void AllowMovement() => canMove = true;

    }
}
