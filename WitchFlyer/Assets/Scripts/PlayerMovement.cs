using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public Vector2 direction;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        direction.Set(InputManager.movement.x, InputManager.movement.y);
        rb.linearVelocity = direction * moveSpeed;
    }
}
