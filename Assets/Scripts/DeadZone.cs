using System.Collections;
using System.Collections.Generic;
using Enemies;
using Managers;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.Player player = collision.gameObject.GetComponent<Player.Player>();

        if (player)
        {
            player.Damage();
            player.Die();
            PlayerManager.Instance.RespawnPlayer();
        }

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null) enemy.Die();
    }
}
