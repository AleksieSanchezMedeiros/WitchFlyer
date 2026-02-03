using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //all enemies have HP, dmg, type
    public int health;
    public int damage;
    public string type;

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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) CheckHealth();
    }

    public void TryTakeFireDamage(int baseDamage)
    {
        if (Time.time < nextFireDamageTime) return;
        nextFireDamageTime = Time.time + fireTickInterval;
        TakeDamage(baseDamage);
    }

    private void DropPowerUp()
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
