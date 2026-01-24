using UnityEngine;
using System.Collections;
public class RusherBat : Enemy
{

    //change these as needed
    //rush speed is the speed at which it flies
    //bounce force is how much it bounces back
    //stun duration is how long before it tries again so also works as cooldown

    public float rushSpeed = 8f;
    public float bounceForce = 5f;
    public float stunDuration = 1.2f;

    private Transform player;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private bool isStunned = false;

    private Vector2 rushTarget;

    //rb and player
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Attack();

        // Check if bat has passed the right boundary
        if (isAttacking && transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect)
        {
            rb.linearVelocity = Vector2.zero;
            StopAllCoroutines();
            StartCoroutine(StunRoutine());
        }
    }   

    private void Awake(){
        transform.parent = null;
    }

    public override void Move()
    {
        // Bats only move when attacking so movement is handled in Attack()
    }

    public override void Attack()
    {
        //dont do anything if already attacking or stunned
        //player check if player doesnt exist in scene
        if (isAttacking || isStunned || player == null) return;

        StartCoroutine(RushAttack());
    }

    private IEnumerator RushAttack()
{
    isAttacking = true;

    // lock direction once
    Vector2 rushDirection = (player.position - transform.position).normalized;

    // clear old movement
    rb.linearVelocity = Vector2.zero;

    // apply burst
    rb.AddForce(rushDirection * rushSpeed, ForceMode2D.Impulse);

    // rush forever until collision stops it
    while (isAttacking && !isStunned)
    {
        yield return null;
    }
}

    private IEnumerator StunRoutine()
    {
        isStunned = true;

        yield return new WaitForSeconds(stunDuration);
        
        rb.linearVelocity = Vector2.zero;
        isStunned = false;
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // HIT PLAYER
        if (isAttacking && collision.gameObject.CompareTag("Player"))
        {
            // DEAL DAMAGE HERE
            /*
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph) ph.TakeDamage(dmg);
            */

            // bounce backwards (opposite of rush direction)
            Vector2 bounceDir = -rb.linearVelocity.normalized;
        
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(bounceDir * bounceForce, ForceMode2D.Impulse);

            StopAllCoroutines();
            StartCoroutine(StunRoutine());
            return;
        }

        // HIT CAMERA BOUNDARY (missed attack)
        if (isAttacking && collision.gameObject.CompareTag("MainCamera"))
        {
            // stop immediately
            rb.linearVelocity = Vector2.zero;

            StopAllCoroutines();
            StartCoroutine(StunRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillWall")) {
            Destroy(gameObject);
        }
    }
}