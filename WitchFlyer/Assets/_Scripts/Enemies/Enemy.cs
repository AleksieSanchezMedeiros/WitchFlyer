using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int damage;
    public EnemyElement element;

    [Header("Fire Tick Gate")]
    [SerializeField] private float fireTickInterval = 0.2f;
    private float nextFireDamageTime;

    [Header("Power Up Drop")]
    [SerializeField][Range(0, 1)] private float dropRate;
    [SerializeField] private GameObject powerUpPrefab;

    void Update()
    {
        Move();
        Attack();
    }

    public virtual void CheckHealth()
    {
        if (health <= 0) {
            DropPowerUp();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage, EnemyElement attackElement)
    {
        float elementalMultiplier = GetElementMultiplier(attackElement, element);
        int finalDamage = Mathf.RoundToInt(damage * elementalMultiplier);

        health -= finalDamage;
        if (health <= 0) CheckHealth();
    }

    public void TryTakeFireDamage(int baseDamage)
    {
        if (Time.time < nextFireDamageTime) return;
        nextFireDamageTime = Time.time + fireTickInterval;
        TakeDamage(baseDamage, EnemyElement.Fire);
    }

    private static float GetElementMultiplier(EnemyElement attack, EnemyElement enemy)
    {
        if (attack == enemy) return 1f;

        return (attack, enemy) switch {
            (EnemyElement.Fire, EnemyElement.Water) => 0.5f,
            (EnemyElement.Fire, EnemyElement.Lightning) => 1.5f,

            (EnemyElement.Water, EnemyElement.Lightning) => 0.5f,
            (EnemyElement.Water, EnemyElement.Fire) => 1.5f,

            (EnemyElement.Lightning, EnemyElement.Fire) => 0.5f,
            (EnemyElement.Lightning, EnemyElement.Water) => 1.5f,

            _ => 1f
        };
    }

    protected void DropPowerUp()
    {
        float randomFloat = Random.Range(0, 1);
        if (randomFloat < dropRate) {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }

    //make move and attack be methods that classes that derive from this one have to implement
    public abstract void Move();
    public abstract void Attack();
}

public enum EnemyElement
{
    Fire,
    Water,
    Lightning,
    NonElemental
}
