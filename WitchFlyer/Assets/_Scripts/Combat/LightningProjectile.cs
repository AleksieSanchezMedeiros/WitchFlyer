using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debugDraw;

    [SerializeField] private float speed = 18f;
    [SerializeField] private float maxTravelDistance = 8f;
    [SerializeField] private float chainRange = 5f;
    private int damage;
    private int remainingHits;
    private Vector2 direction;
    private Vector2 spawnPos;
    private HashSet<int> hitIds;

   

    public void Initialize(Vector2 direction, int damage, int remainingHits, HashSet<int> hitIds) 
    {
        this.direction = (direction.sqrMagnitude < 0.0001f) ? Vector2.right : direction.normalized;
        this.damage = damage;
        this.remainingHits = remainingHits;
        this.hitIds = hitIds ?? new HashSet<int>();

        spawnPos = transform.position;

        transform.right = this.direction;

        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (Vector2.Distance(spawnPos, transform.position) >= maxTravelDistance)
            Destroy(gameObject);

        if (debugDraw)
            Debug.DrawRay(transform.position, direction * 0.5f, Color.yellow);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null) return;

        int id = enemy.GetInstanceID();
        if (hitIds.Contains(id)) return;

        hitIds.Add(id);

        int totalDamage = damage + Player.Instance.GetSongBonus(Element.Lightning);
        enemy.TakeDamage(totalDamage);

        remainingHits--;
        if (remainingHits <= 0) {
            Destroy(gameObject);
            return;
        }

        Enemy next = FindNextTarget(enemy.transform.position);
        if (next == null) {
            Destroy(gameObject);
            return;
        }
        Debug.Log("FOUDN ENEMY!");

        Vector2 nextDir = ((Vector2)next.transform.position - (Vector2)enemy.transform.position).normalized;

        LightningProjectile nextProj = Instantiate(this, enemy.transform.position, Quaternion.identity);
        nextProj.Initialize(nextDir, damage, remainingHits, hitIds);
        Destroy(gameObject);
    }

    private Enemy FindNextTarget(Vector2 origin)
    {
        Debug.Log("FINDING NEXT TARGET");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(origin, chainRange);
        int enemyCount = hitEnemies.Length;
        if (enemyCount <= 0) return null;

        Enemy best = null;
        float bestDistSq = float.PositiveInfinity;

        for (int i = 0; i < enemyCount; i++) {
            Debug.Log("LOOP COUNT: " + i);
            Collider2D collider = hitEnemies[i];
            if (collider == null) continue;
            Debug.Log("COLLIDERD NOT NULL");
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy == null) continue;
            Debug.Log("ENEMY COMPONENT PRESENT");
            int id = enemy.GetInstanceID();
            if (hitIds.Contains(id)) continue;
            
            Debug.Log("BEFORE CHECK");
            float distance = ((Vector2)enemy.transform.position - origin).sqrMagnitude;
            if (distance < bestDistSq) { 
                bestDistSq = distance;
                best = enemy;
                Debug.Log("CHECK");
            }
        }
        Debug.Log("HERE");

        return best;
    }

    private void OnDrawGizmosSelected()
    {
        if (!debugDraw) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chainRange);
    }
}
