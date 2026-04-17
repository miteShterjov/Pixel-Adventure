using UnityEngine;

namespace Camera
{
    public class LevelCameraTrigger : MonoBehaviour
    {
        private LevelCamera levelCamera;

        private void Awake()
        {
            levelCamera = GetComponentInParent<LevelCamera>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.TryGetComponent<Player.Player>(out var player)) return;
            levelCamera.EnableCamera(true);
            levelCamera.SetNewTarger(player.transform);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Player.Player>()) levelCamera.EnableCamera(false);
        }
    }
}
