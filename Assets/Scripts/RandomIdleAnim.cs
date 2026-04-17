using UnityEngine;

public class RandomIdleAnim : MonoBehaviour
{
    private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            if (animator == null)
            {
                Debug.LogError("No Animator found in RandomIdleAnim Class on" + name);
                return;
            }

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(stateInfo.fullPathHash, -1, Random.Range(0f, 1f));
        }
}
