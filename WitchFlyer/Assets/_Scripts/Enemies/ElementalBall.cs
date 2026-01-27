using UnityEngine;

public class ElementalBall : Enemy
{
    public float moveSpeed;
    public Vector2 moveDirection = Vector2.left;
    [SerializeField][Range(0, 1)] private float dropRate;
    [SerializeField] private GameObject powerUpPrefab;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    public override void CheckHealth()
    {
        if (health <= 0) {
            DropPowerUp();
            Destroy(gameObject);
        }
    }

    private void DropPowerUp()
    {
        float randomFloat = Random.Range(0, 1);
        if (randomFloat < dropRate) {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillWall")) Destroy(gameObject);
        if (other.CompareTag("Player")) CheckHealth();
        if (other.CompareTag("Bullet")) CheckHealth();
    }

    public override void Attack() { }
}
