using Unity.Cinemachine;
using UnityEngine;

namespace Enemies
{
    public class EnemyRino : Enemy
    {
        [Header("Rino details")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float speedUpRate = .6f;
        [SerializeField] private Vector2 impactPower;

        [Header("Effects")]
        [SerializeField] private ParticleSystem dustFx;
        [SerializeField] private Vector2 cameraImpulseDir;
        
        private float defaultSpeed;
        private CinemachineImpulseSource impulseSource;
        private static readonly int HitWallParam = Animator.StringToHash("hitWall");

        protected override void Start()
        {
            base.Start();

            canMove = false;
            defaultSpeed = moveSpeed;
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        protected override void Update()
        {
            base.Update();
            HandleCharge();
        }
        
        protected override void HandleCollision()
        {
            base.HandleCollision();

            if (isPlayerDetected && isGrounded) canMove = true;
        }

        private void HitWallImpact()
        {
            dustFx.Play();
            impulseSource.DefaultVelocity = new Vector2(cameraImpulseDir.x * facingDir, cameraImpulseDir.y);
            impulseSource.GenerateImpulse();
        }

        private void HandleCharge()
        {
            if (!canMove) return;

            HandleSpeedUp();

            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);

            if (isWallDetected) WallHit();

            if (!isGroundInFrontDetected) TurnAround();
        }

        private void HandleSpeedUp()
        {
            moveSpeed = moveSpeed + (Time.deltaTime * speedUpRate);

            if (moveSpeed >= maxSpeed) maxSpeed = moveSpeed;
        }

        private void TurnAround()
        {
            SpeedReset();
            canMove = false;
            rb.linearVelocity = Vector2.zero;
            Flip();
        }

        private void WallHit()
        {
            canMove = false;

            HitWallImpact();
            SpeedReset();

            anim.SetBool(HitWallParam, true);
            rb.linearVelocity = new Vector2(impactPower.x * -facingDir, impactPower.y);
        }

        private void SpeedReset() => moveSpeed = defaultSpeed;
        
        private void ChargeIsOver()
        {
            anim.SetBool(HitWallParam, false);
            Invoke(nameof(Flip), 1);
        }
    }
}
