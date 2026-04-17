using Managers;
using UnityEngine;

namespace Traps
{
    public class TrapArrow : TrapTrampoline
    {
        [Header("Additional info")]
        [SerializeField] private float cooldown;
        [SerializeField] private bool rotationRight;
        [SerializeField] private float rotationSpeed = 120;
        [Space]
        [SerializeField] private float scaleUpSpeed = 10;
        [SerializeField] private Vector3 targetScale;

        private int direction = -1;
        
        private void Start()
        {
            transform.localScale = new Vector3(.3f, .3f, .3f);
        }


        private void Update()
        {
            HandleScaleUp();
            HandleRotation();
        }

        private void HandleScaleUp()
        {
            if (transform.localScale.x < targetScale.x)
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleUpSpeed * Time.deltaTime);
        }

        private void HandleRotation()
        {
            direction = rotationRight ? -1 : 1;
            transform.Rotate(0, 0, (rotationSpeed * direction) * Time.deltaTime);
        }

        private void DestroyMe()
        {
            GameObject arrowPrefab = ObjectCreator.Instance.arrowPrefab;
            ObjectCreator.Instance.CreateObject(arrowPrefab, transform,false, cooldown);

            Destroy(gameObject);
        }
    }
}
