using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debugDraw;

    [SerializeField] private float speed = 18f;
    [SerializeField] private float maxTravelDistance = 8f;
    [SerializeField] private float chainRange = 5f;
    [SerializeField] private float trailDestroyDelay;
    private int damage;
    private int remainingHits;
    private Vector2 direction;
    private Vector2 spawnPos;
    private HashSet<int> hitIds;

    private SpriteRenderer spriteRenderer;
    private Collider2D projectileCollider;
    private TrailRenderer trail;
    private bool isDying;

    public void Initialize(Vector2 direction, int damage, int remainingHits, HashSet<int> hitIds) 
    {
        this.direction = (direction.sqrMagnitude < 0.0001f) ? Vector2.right : direction.normalized;
        this.damage = damage;
        this.remainingHits = remainingHits;
        this.hitIds = hitIds ?? new HashSet<int>();

        spawnPos = transform.position;

        transform.right = this.direction;

        projectileCollider = GetComponent<Collider2D>();
        projectileCollider.isTrigger = true;

        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (isDying) return;

        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (Vector2.Distance(spawnPos, transform.position) >= maxTravelDistance)
            Destroy(gameObject);

        if (debugDraw)
            Debug.DrawRay(transform.position, direction * 0.5f, Color.yellow);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDying) return;

        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null) return;

        int id = enemy.GetInstanceID();
        if (hitIds.Contains(id)) return;

        hitIds.Add(id);

        int totalDamage = damage + Player.Instance.GetSongBonus(Element.Lightning);
        enemy.TakeDamage(totalDamage, EnemyElement.Lightning);

        remainingHits--;
        if (remainingHits <= 0) {
            DestroyThisObject(other);
            return;
        }

        Vector2 hitPoint = other.ClosestPoint(transform.position);
        Enemy next = FindNextTarget(hitPoint);
        if (next == null) {
            DestroyThisObject(other);
            return;
        }

        Vector2 nextDir = ((Vector2)next.transform.position - (Vector2)enemy.transform.position).normalized;

        LightningProjectile nextProj = Instantiate(this, enemy.transform.position, Quaternion.identity);
        nextProj.Initialize(nextDir, damage, remainingHits, hitIds);

        DestroyThisObject(other);
    }

    private Enemy FindNextTarget(Vector2 origin)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(origin, chainRange);
        int enemyCount = hitEnemies.Length;
        if (enemyCount <= 0) return null;

        Enemy best = null;
        float bestDistSq = float.PositiveInfinity;

        for (int i = 0; i < enemyCount; i++) {
            Collider2D collider = hitEnemies[i];
            if (collider == null) continue;

            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy == null) continue;

            int id = enemy.GetInstanceID();
            if (hitIds.Contains(id)) continue;

            float distance = ((Vector2)enemy.transform.position - origin).sqrMagnitude;
            if (distance < bestDistSq) { 
                bestDistSq = distance;
                best = enemy;
            }
        }

        return best;
    }

    private void DestroyThisObject(Collider2D other = null)
    {
        if (isDying) return;
        isDying = true;

        spriteRenderer.enabled = false;
        projectileCollider.enabled = false;

        if (other != null) {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null) transform.position = enemy.transform.position;
            else transform.position = other.bounds.center;
        }

        Invoke(nameof(DelayedDestroy), trailDestroyDelay);
    }
    private void DelayedDestroy() => Destroy(gameObject);

    private void OnDrawGizmosSelected()
    {
        if (!debugDraw) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chainRange);
    }
}
