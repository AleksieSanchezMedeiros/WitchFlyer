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

    void Update()
    {
        Move();
        Attack();
    }

    public virtual void CheckHealth()
    {
        if (health <= 0) Destroy(gameObject);
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

    //make move and attack be methods that classes that derive from this one have to implement
    public abstract void Move();
    public abstract void Attack();
}
