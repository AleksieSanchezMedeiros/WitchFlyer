using UnityEngine;

public class ElementalBall : Enemy
{
    public float moveSpeed;
    public Vector2 moveDirection = Vector2.left;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillWall")) Destroy(gameObject);
        if (other.CompareTag("Player")) CheckHealth();
        if (other.CompareTag("Bullet")) CheckHealth();
    }

    public override void Attack() { }
}
