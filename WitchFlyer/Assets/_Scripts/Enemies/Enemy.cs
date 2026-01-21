using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //all enemies have HP, dmg, type
    public int HP;
    public int dmg;
    public string type;

    void Update()
    {
        Move();
        Attack();
    }

    public void CheckHealth()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0) CheckHealth();
    }

    //make move and attack be methods that classes that derive from this one have to implement
    public abstract void Move();
    public abstract void Attack();
}
