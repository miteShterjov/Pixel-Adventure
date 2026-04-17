using Managers;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Header("Fruit type")]
    [SerializeField] private FruitType fruitType;
    [SerializeField] private GameObject pickupVfx;

    private GameManager gameManager;
    protected Animator anim;
    protected SpriteRenderer sr;
    
    private static readonly int FruitIndex = Animator.StringToHash("fruitIndex");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;
        SetRandomLookIfNeeded();
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player.Player player = collision.GetComponent<Player.Player>();

        if (!player) return;
        gameManager.AddFruit();
        AudioManager.Instance.PlaySfx(8);
        Destroy(gameObject);

        GameObject newFx = Instantiate(pickupVfx, transform.position, Quaternion.identity);
    }

    private void SetRandomLookIfNeeded()
    {
        if (!gameManager.FruitsHaveRandomLook())
        {
            UpdateFruitVisuals();
            return;
        }
        int randomIndex = Random.Range(0, 8);
        anim.SetFloat(FruitIndex, randomIndex);
    }
    
    private void UpdateFruitVisuals() => anim.SetFloat(FruitIndex, (int)fruitType);
}

public enum FruitType { Apple,Banana,Cherry,Kiwi,Melon,Orange,Pineapple,Strawberry}