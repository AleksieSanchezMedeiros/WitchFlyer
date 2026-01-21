using UnityEngine;

public class HailProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float shardLifetime = 2f;

    [Header("Damage")]
    [SerializeField] private int directHitDamage = 2;
    [SerializeField] private int shardDamage = 1;

    [Header("Explosion Visuals (optional)")]
    [SerializeField] private bool spawnShards = false;
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private int shardCount = 6;
    [SerializeField] private float shardSpeed = 12f;

    private float lifeTimer;

    private void OnEnable()
    {
        lifeTimer = lifetime;
    }

    private void Update()
    {
        transform.position += transform.right * (speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleImpact(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleImpact(collision.gameObject);
    }

    private void HandleImpact(GameObject hitObject)
    {
        DealDamage(hitObject, directHitDamage);

        Explode();

        ReturnToPool();
    }

    private void Explode()
    {
        if (spawnShards && shardPrefab != null && shardCount > 0) {
            float angleStep = 360f / shardCount;
            for (int i = 0; i < shardCount; i++) {
                float angle = i * angleStep;
                Quaternion rot = Quaternion.Euler(0f, 0f, angle);

                Vector3 offset = rot * Vector3.right * 0.15f; // slight offset to avoid instant re-collision
                GameObject shard = Instantiate(shardPrefab, transform.position + offset, rot);

                HailShard shardMover = shard.GetComponent<HailShard>();
                if (shardMover != null) {
                    shardMover.SetValues(shardSpeed, shardDamage, shardLifetime);
                }
            }
        }
    }


    private void DealDamage(GameObject target, int amount)
    {
        if (amount <= 0) return;
        target.SendMessage("TakeDamage", amount, SendMessageOptions.DontRequireReceiver);
    }

    private void ReturnToPool()
    {
        // Return to pool appropriately
        gameObject.SetActive(false);
    }

//#if UNITY_EDITOR
//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.DrawWireSphere(transform.position, explosionRadius);
//    }
//#endif
}
