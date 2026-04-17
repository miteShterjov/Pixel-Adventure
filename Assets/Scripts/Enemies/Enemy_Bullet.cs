using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyBullet : MonoBehaviour
    {
        [Header( "Layer Names")]
        [SerializeField] private string playerLayerName = "Player";
        [SerializeField] private string groundLayerName = "Ground";
        
        private Rigidbody2D rb;
        private SpriteRenderer sr;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }

        public void FlipSprite() => sr.flipX = !sr.flipX;
        public void SetVelocity(Vector2 velocity) => rb.linearVelocity = velocity;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
            {
                Player.Player player = collision.GetComponent<Player.Player>();
                player.Damage();
                player.Knockback(transform.position.x);
                Destroy(gameObject);
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName)) Destroy(gameObject);
        }
    }
}
