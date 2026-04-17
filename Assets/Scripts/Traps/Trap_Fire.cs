using System.Collections;
using UnityEngine;

namespace Traps
{
    public class TrapFire : MonoBehaviour
    {
        [SerializeField] private float offDuration;
        [SerializeField] private TrapFireButton fireButton;
        
        private Animator anim;
        private CapsuleCollider2D fireCollider;
        private bool isActive;
        
        private static readonly int ActiveParam = Animator.StringToHash("active");

        private void Awake()
        {
            anim = GetComponent<Animator>();
            fireCollider = GetComponent<CapsuleCollider2D>();
        }
        
        private void Start()
        {
            if (!fireButton) Debug.LogWarning("You don't have fire button on " + gameObject.name + "!");
            SetFire(true);
        }

        public void SwitchOffFire()
        {
            if (!isActive) return;

            StartCoroutine(FireCo());
        }
        
        private IEnumerator FireCo()
        {
            SetFire(false);
            yield return new WaitForSeconds(offDuration);
            SetFire(true);
        }

        private void SetFire(bool active)
        {
            anim.SetBool(ActiveParam, active);
            fireCollider.enabled = active;
            isActive = active;
        }
    }
}
