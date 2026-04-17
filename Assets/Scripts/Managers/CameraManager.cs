using Unity.Cinemachine;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [Header("Screen Shake")]
        [SerializeField] private Vector2 shakeVelocity;

        private CinemachineImpulseSource impulseSource;

        private void Awake()
        {
            Instance = this;

            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void ScreenShake(float shakeDirection)
        {
            impulseSource.DefaultVelocity = new Vector2(shakeVelocity.x * shakeDirection, shakeVelocity.y);
            impulseSource.GenerateImpulse();
        }
    }
}
