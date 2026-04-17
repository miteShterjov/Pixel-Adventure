using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemies
{
    public class EnemyBulletBee : MonoBehaviour
    {
        [Header("Bullet details")]
        [SerializeField] private GameObject pickupVfx;
        [SerializeField] private float wayPointUpdateCooldown;
        
        private Transform target;
        private readonly List<Vector3> wayPoints = new List<Vector3>();
        private int wayIndex;
        private float speed;

        public void SetupBullet(Transform newTarget, float newSpeed, float lifeDuration)
        {
            speed = newSpeed;
            target = newTarget;

            transform.up = transform.position - target.position;

            StartCoroutine(AddWayPointCo());
            Destroy(gameObject,lifeDuration);
        }

        private void Update()
        {
            if (wayPoints.Count <= 0 || wayIndex >= wayPoints.Count)
                return;

            transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayIndex], speed * Time.deltaTime);

            if (!(Vector2.Distance(transform.position, wayPoints[wayIndex]) < .1f)) return;
            wayIndex++;
            if (wayIndex < wayPoints.Count) transform.up = transform.position - wayPoints[wayIndex];
        }

        private IEnumerator AddWayPointCo()
        {
            while (target)
            {
                AddWayPoint();
                yield return new WaitForSeconds(wayPointUpdateCooldown);
            }
        }

        private void AddWayPoint()
        {
            if (!target) return;

            if (wayPoints.Any(wayPoint => wayPoint == target.position)) return;
            
            wayPoints.Add(target.position);
        }

        private void OnDestroy()
        {
            GameObject newFx = Instantiate(pickupVfx, transform.position, Quaternion.identity);
            newFx.transform.localScale = new Vector3(.6f, .6f, .6f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            const float destroyDelay = .2f;

            if (!collision.gameObject.TryGetComponent(out Player.Player player)) return;
            player.Damage();
            player.Knockback(transform.position.x);
            Destroy(gameObject, destroyDelay);
        }
    }
}
