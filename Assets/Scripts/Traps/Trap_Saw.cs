using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traps
{
    public class TrapSaw : MonoBehaviour
    {
        public int wayPointIndex = 1;
        public int moveDirection = 1;
        
        [Header("Saw details")]
        [SerializeField] private float moveSpeed = 3;
        [SerializeField] private float cooldown = 1;
        [SerializeField] private Transform[] wayPoint;
        
        private Animator anim;
        private SpriteRenderer sr;
        private Vector3[] wayPointPosition;
        private bool canMove = true;
        
        private static readonly int ActiveParam = Animator.StringToHash("active");
        
        private void Awake()
        {
            anim = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
        }
        
        private void Start()
        {
            UpdateWaypointsInfo();
            transform.position = wayPointPosition[0];
        }
        
        private void Update()
        {
            anim.SetBool(ActiveParam, canMove);

            if (canMove == false) return;

            transform.position = Vector2.MoveTowards(transform.position, wayPointPosition[wayPointIndex], moveSpeed * Time.deltaTime);

            if (!(Vector2.Distance(transform.position, wayPointPosition[wayPointIndex]) < .1f)) return;
            
            if (wayPointIndex == wayPointPosition.Length - 1 || wayPointIndex == 0)
            {
                moveDirection = moveDirection * -1;
                StartCoroutine(StopMovement(cooldown));
            }

            wayPointIndex = wayPointIndex + moveDirection;
        }

        private void UpdateWaypointsInfo()
        {
            List<TrapSawWaypoint> wayPointList = new List<TrapSawWaypoint>(GetComponentsInChildren<TrapSawWaypoint>());

            if (wayPointList.Count != wayPoint.Length)
            {
                wayPoint = new Transform[wayPointList.Count];

                for (int i = 0; i < wayPointList.Count; i++)
                {
                    wayPoint[i] = wayPointList[i].transform;
                }
            }

            wayPointPosition = new Vector3[wayPoint.Length];

            for (int i = 0; i < wayPoint.Length; i++)
            {
                wayPointPosition[i] = wayPoint[i].position;
            }
        }

        private IEnumerator StopMovement(float delay)
        {
            canMove = false;

            yield return new WaitForSeconds(delay);

            canMove = true;
            sr.flipX = !sr.flipX;
        }
    }
}
