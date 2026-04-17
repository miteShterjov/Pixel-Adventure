using UnityEngine;

namespace Traps
{
    public class TrapTrampoline : MonoBehaviour
    {
        [SerializeField] private float pushPower;
        [SerializeField] private float duration = .5f;

        private Animator anim;
        private static readonly int ActivateTrigger = Animator.StringToHash("activate");

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player.Player player = collision.gameObject.GetComponent<Player.Player>();

            if (!player) return;
            
            player.Push(transform.up * pushPower,duration);
            anim.SetTrigger(ActivateTrigger);
        }
    }
}
