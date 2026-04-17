using Managers;
using UnityEngine;

namespace Checkpoint
{
    [RequireComponent(typeof(Animator))]
    public class Checkpoint : MonoBehaviour
    {
        private Animator anim;
        private bool active;
        private static readonly int ActivateTrigger = Animator.StringToHash("activate");

        [SerializeField] private bool canBeReactivated;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            canBeReactivated = GameManager.Instance.canReactivate;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (active && !canBeReactivated) return;
            if (collision.GetComponent<Player.Player>()) ActivateCheckpoint();
        }

        private void ActivateCheckpoint()
        {
            active = true;
            anim.SetTrigger(ActivateTrigger);
            PlayerManager.Instance.UpdateRespawnPosition(transform);
        }
    }
}
