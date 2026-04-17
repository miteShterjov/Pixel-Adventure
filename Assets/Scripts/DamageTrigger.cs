using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.Player player = collision.gameObject.GetComponent<Player.Player>();

        if (!player) return;
        player.Damage();
        player.Knockback(transform.position.x);
    }
}
