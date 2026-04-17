using UnityEngine;

namespace Traps
{
    public class TrapFireButton : MonoBehaviour
    {
        private Animator anim;
        private TrapFire trapFire;
    
        private static readonly int ActivateTrigger = Animator.StringToHash("activate");

        private void Awake()
        {
            anim = GetComponent<Animator>();
            trapFire = GetComponentInParent<TrapFire>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.GetComponent<Player.Player>()) return;
            anim.SetTrigger(ActivateTrigger);
            trapFire.SwitchOffFire();
        }
    }
}
