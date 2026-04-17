using UnityEngine;

namespace Enemies
{
    public class EnemyPlant : Enemy
    {
        [Header("Plant details")]
        [SerializeField] private EnemyBullet bulletPrefab;
        [SerializeField] private Transform gunPoint;
        [SerializeField] private float bulletSpeed = 7;
        [SerializeField] private float attackCooldown = 1.5f;
        
        private static readonly int AttackParam = Animator.StringToHash("attack");
    
        private float lastTimeAttacked;

        protected override void Update()
        {
            base.Update();

            bool canAttack = Time.time > lastTimeAttacked + attackCooldown;

            if (isPlayerDetected && canAttack) Attack();
        }
        
        protected override void HandleAnimator()
        {
        }

        private void Attack()
        {
            lastTimeAttacked = Time.time;
            anim.SetTrigger(AttackParam);
        }

        private void CreateBullet()
        {
            EnemyBullet newBullet = Instantiate(bulletPrefab,gunPoint.position,Quaternion.identity);

            Vector2 bulletVelocity = new Vector2(facingDir * bulletSpeed, 0);
            newBullet.SetVelocity(bulletVelocity);

            Destroy(newBullet.gameObject, 10);
        }
    }
}
