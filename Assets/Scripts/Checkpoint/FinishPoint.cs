using Managers;
using UnityEngine;

namespace Checkpoint
{
    [RequireComponent(typeof(Animator))]
    public class FinishPoint : MonoBehaviour
    {
        private Animator anim;
        private static readonly int ActivateTrigger = Animator.StringToHash("activate");

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.GetComponent<Player.Player>()) return;
            
            AudioManager.Instance.PlaySfx(2);
            anim.SetTrigger(ActivateTrigger);
            GameManager.Instance.LevelFinished();
        }
    }
}
