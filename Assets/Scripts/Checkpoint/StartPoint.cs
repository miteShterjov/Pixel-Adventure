using UnityEngine;

namespace Checkpoint
{
    [RequireComponent(typeof(Animator))]
    public class StartPoint : MonoBehaviour
    {
        private Animator anim;
        private static readonly int ActivateTrigger = Animator.StringToHash("activate");
    
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Player.Player>()) anim.SetTrigger(ActivateTrigger);
        }
    }
}
