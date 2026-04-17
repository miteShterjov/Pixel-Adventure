using UnityEngine;

public class MenuCharacter : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;

    private Vector3 destination;
    private Animator anim;
    private bool isMoving;
    private int facingDir = 1;
    private bool facingRight = true;
    private static readonly int MovingParam = Animator.StringToHash("isMoving");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool(MovingParam, isMoving);

        if (!isMoving) return;
        transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * speed);

        if (Vector2.Distance(transform.position, destination) < .1f) isMoving = false;
    }

    public void MoveTo(Transform newDestination)
    {
        destination = newDestination.position;
        destination.y = transform.position.y;

        isMoving = true;
        HandleFlip(destination.x);
    }

    private void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight)
            Flip();
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
}
