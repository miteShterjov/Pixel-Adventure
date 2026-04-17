using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        public bool IsKnocked { get; private set; }

        [Header("Gameplay")]
        [SerializeField] private GameObject fruitDrop;
        [SerializeField] private DifficultyType gameDifficulty;
        [Header("Knockback")]
        [SerializeField] private float knockbackDuration = 1;
        [SerializeField] private Vector2 knockbackPower;
        [Header("Player Visuals")]
        [SerializeField] private GameObject deathVfx;

        private GameManager gameManager;
        private Rigidbody2D rb;
        private CapsuleCollider2D cd;
        private PlayerAnimationController animationController;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            cd = GetComponent<CapsuleCollider2D>();
            animationController = GetComponent<PlayerAnimationController>();
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            UpdateGameDifficulty();
        }

        public void Damage()
        {
            switch (gameDifficulty)
            {
                case DifficultyType.Normal:
                {
                    if (gameManager.FruitsCollected() <= 0)
                    {
                        Die();
                        gameManager.RestartLevel();
                    }
                    else
                    {
                        ObjectCreator.Instance.CreateObject(fruitDrop, transform, true);
                        gameManager.RemoveFruit();
                    }
                    return;
                }
                
                case DifficultyType.Hard:
                    Die();
                    gameManager.RestartLevel();
                    break;
                
                case DifficultyType.Easy:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Knockback(float sourceDamageXPosition)
        {
            float knockbackDir = 1;

            if (transform.position.x < sourceDamageXPosition) knockbackDir = -1;

            if (IsKnocked) return;

            AudioManager.Instance.PlaySfx(9);
            CameraManager.Instance.ScreenShake(knockbackDir);
            StartCoroutine(KnockbackCo());

            rb.linearVelocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
        }

        public void Die()
        {
            AudioManager.Instance.PlaySfx(0);
            GameObject newDeathVfx = Instantiate(deathVfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void Push(Vector2 direction, float duration = 0)
        {
            StartCoroutine(PushCo(direction, duration));
        }

        public void RespawnFinished(bool finished)
        {
            if (finished)
            {
                rb.gravityScale = 1; // Will be reset by JumpController
                cd.enabled = true;
                AudioManager.Instance.PlaySfx(11);
            }
            else
            {
                rb.gravityScale = 0;
                cd.enabled = false;
            }
        }
        
        private IEnumerator KnockbackCo()
        {
            IsKnocked = true;
            animationController.SetKnockedAnimation(true);

            yield return new WaitForSeconds(knockbackDuration);

            IsKnocked = false;
            animationController.SetKnockedAnimation(false);
        }
        
        private IEnumerator PushCo(Vector2 direction, float duration)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction, ForceMode2D.Impulse);

            yield return new WaitForSeconds(duration);
        }
        
        private void UpdateGameDifficulty()
        {
            DifficultyManager difficultyManager = DifficultyManager.Instance;

            if (difficultyManager != null) gameDifficulty = difficultyManager.difficulty;
        }

    }
}
