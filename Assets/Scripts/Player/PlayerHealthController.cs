using System.Collections;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private GameObject fruitDrop;
    [SerializeField] private DifficultyType gameDifficulty;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private CapsuleCollider2D cd;
    private PlayerAnimationController animationController;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 1;
    [SerializeField] private Vector2 knockbackPower;

    [Header("Player Visuals")]
    [SerializeField] private GameObject deathVfx;

    private bool isKnocked;

    public bool IsKnocked => isKnocked;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        UpdateGameDifficulty();
    }

    private void UpdateGameDifficulty()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;

        if (difficultyManager != null)
            gameDifficulty = difficultyManager.difficulty;
    }

    public void Damage()
    {
        if (gameDifficulty == DifficultyType.Normal)
        {
            if (gameManager.FruitsCollected() <= 0)
            {
                Die();
                gameManager.RestartLevel();
            }
            else
            {
                ObjectCreator.instance.CreateObject(fruitDrop, transform, true);
                gameManager.RemoveFruit();
            }

            return;
        }

        if (gameDifficulty == DifficultyType.Hard)
        {
            Die();
            gameManager.RestartLevel();
        }
    }

    public void Knockback(float sourceDamageXPosition)
    {
        float knockbackDir = 1;

        if (transform.position.x < sourceDamageXPosition)
            knockbackDir = -1;

        if (isKnocked)
            return;

        AudioManager.instance.PlaySFX(9);
        CameraManager.instance.ScreenShake(knockbackDir);
        StartCoroutine(KnockbackRoutine());

        rb.linearVelocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnocked = true;
        animationController.SetKnockedAnimation(true);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
        animationController.SetKnockedAnimation(false);
    }

    public void Die()
    {
        AudioManager.instance.PlaySFX(0);
        GameObject newDeathVfx = Instantiate(deathVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Push(Vector2 direction, float duration = 0)
    {
        StartCoroutine(PushCouroutine(direction, duration));
    }

    private IEnumerator PushCouroutine(Vector2 direction, float duration)
    {
        // Disable player control is handled by main Player script
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        // Re-enable player control is handled by main Player script
    }

    public void RespawnFinished(bool finished)
    {
        if (finished)
        {
            rb.gravityScale = 1; // Will be reset by JumpController
            cd.enabled = true;
            AudioManager.instance.PlaySFX(11);
        }
        else
        {
            rb.gravityScale = 0;
            cd.enabled = false;
        }
    }
}
