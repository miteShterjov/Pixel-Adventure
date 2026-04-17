using Enemies;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private bool canBeControlled;

        private PlayerMovementController movementController;
        private PlayerCollisionDetector collisionDetector;
        private PlayerAnimationController animationController;
        private PlayerJumpController jumpController;
        private PlayerHealthController healthController;
        private InputSystem_Actions inputActions;

        private Vector2 moveInput;

        private void Awake()
        {
            movementController = GetComponent<PlayerMovementController>();
            collisionDetector = GetComponent<PlayerCollisionDetector>();
            animationController = GetComponent<PlayerAnimationController>();
            jumpController = GetComponent<PlayerJumpController>();
            healthController = GetComponent<PlayerHealthController>();

            inputActions = new InputSystem_Actions();
        }

        private void Start()
        {
            RespawnFinished(false);
            animationController.UpdateSkin();
        }

        private void OnEnable()
        {
            inputActions.Player.Enable();
            inputActions.Player.Move.performed += OnMovePerformed;
            inputActions.Player.Move.canceled += OnMoveCanceled;
            inputActions.Player.Jump.performed += OnJumpPerformed;
        }

        private void OnDisable()
        {
            inputActions.Player.Move.performed -= OnMovePerformed;
            inputActions.Player.Move.canceled -= OnMoveCanceled;
            inputActions.Player.Jump.performed -= OnJumpPerformed;
            inputActions.Player.Disable();
        }

        // Named handlers so they can be properly unsubscribed
        private void OnMovePerformed(InputAction.CallbackContext ctx)
            => movementController.HandleInput(ctx.ReadValue<Vector2>());

        private void OnMoveCanceled(InputAction.CallbackContext ctx)
            => movementController.HandleInput(Vector2.zero);

        private void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
            jumpController.JumpButton();
            jumpController.RequestBufferJump();
        }

        private void Update()
        {
            collisionDetector.UpdateCollisions();
            jumpController.UpdateAirborneStatus();

            // Read move input every frame (not subscribe — just read)
            moveInput = inputActions.Player.Move.ReadValue<Vector2>();

            if (!canBeControlled)
            {
                animationController.UpdateAnimations();
                animationController.UpdateGroundedAnimation(collisionDetector.IsGrounded);
                animationController.UpdateWallDetectedAnimation(collisionDetector.IsWallDetected);
                return;
            }

            if (healthController.IsKnocked) return;

            HandleEnemyDetection();
            movementController.HandleWallSlide(moveInput);
            movementController.HandleMovement();
            movementController.HandleFlip();
            animationController.UpdateAnimations();
            animationController.UpdateGroundedAnimation(collisionDetector.IsGrounded);
            animationController.UpdateWallDetectedAnimation(collisionDetector.IsWallDetected);
        }

        public void Damage() => healthController.Damage();

        public void RespawnFinished(bool finished)
        {
            healthController.RespawnFinished(finished);

            if (finished)
            {
                jumpController.ResetGravityScale();
                canBeControlled = true;
            }
            else
            {
                jumpController.SetGravityScale(0);
                canBeControlled = false;
            }
        }

        public void UpdateSkin() => animationController.UpdateSkin();

        public void Knockback(float sourceDamageXPosition)
            => healthController.Knockback(sourceDamageXPosition);

        public void Die() => healthController.Die();

        public void Push(Vector2 direction, float duration = 0)
            => healthController.Push(direction, duration);

        private void HandleEnemyDetection()
        {
            if (!jumpController.IsAirborne) return;
            if (GetComponent<Rigidbody2D>().linearVelocity.y >= 0) return;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                collisionDetector.EnemyCheck.position,
                collisionDetector.EnemyCheckRadius,
                collisionDetector.WhatIsEnemy);

            foreach (var enemy in colliders)
            {
                Enemy newEnemy = enemy.GetComponent<Enemy>();
                if (!newEnemy) continue;
                AudioManager.Instance.PlaySfx(1);
                newEnemy.Die();
                jumpController.EnableDoubleJump();
                jumpController.JumpButton();
            }
        }
    }
}