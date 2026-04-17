using UnityEngine;

namespace Player
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        private global::Player.Player player;

        private void Awake()
        {
            player = GetComponentInParent<global::Player.Player>();
        }

        public void FinishRespawn() => player.RespawnFinished(true);
    }
}
