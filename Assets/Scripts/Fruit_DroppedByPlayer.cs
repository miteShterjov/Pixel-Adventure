using System.Collections;
using UnityEngine;

public class FruitDroppedByPlayer : Fruit
{
    [Header("Fruit details")]
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Color transparentColor;
    [SerializeField] private float[] waitTime;
    private bool canPickUp;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(BlinkCo());
    }

    private void Update()
    {
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;  
    }

    private IEnumerator BlinkCo()
    {
        anim.speed = 0;

        foreach (float seconds in waitTime)
        {
            ToggleSpeedAndColor(transparentColor);

            yield return new WaitForSeconds(seconds);

            ToggleSpeedAndColor(Color.white);

            yield return new WaitForSeconds(seconds);
        }

        velocity.x = 0;
        anim.speed = 1;
        canPickUp = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (canPickUp == false) 
            return;

        base.OnTriggerEnter2D(collision);
    }
    
    private void ToggleSpeedAndColor(Color color)
    {
        velocity.x = velocity.x * -1;
        sr.color = color;
    }
}
