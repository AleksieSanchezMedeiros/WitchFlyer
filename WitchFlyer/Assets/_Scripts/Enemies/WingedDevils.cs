using UnityEngine;

public class WingedDevils : Enemy
{
    [Header("Movement")]
    public float speedRight = 3f;
    public float bobAmplitude = 1.5f;
    public float bobFrequency = 2f;

    [Header("Attack")]
    public float shootCooldown = 1.5f;

    private bool canAttack = true;
    private float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void Awake(){
        transform.parent = null;
    }

    public override void Move()
    {
        // Move (against camera scroll)
        transform.Translate(Vector2.left * -speedRight * Time.deltaTime);

        // Vertical flying motion
        float yOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(
            transform.position.x,
            startY + yOffset,
            transform.position.z
        );
    }

    public override void Attack()
    {
        if (!canAttack)
            return;

        canAttack = false;

        // FIRE PROJECTILE HERE

        Invoke(nameof(ResetAttack), shootCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    // Camera collider handles despawn
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("KillWall"))
        {
            Destroy(gameObject);
        }
    }
}
