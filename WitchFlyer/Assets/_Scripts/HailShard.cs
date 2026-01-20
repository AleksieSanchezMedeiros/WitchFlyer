using UnityEngine;

public class HailShard : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifetime;

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    public void SetValues(float speed, int damage, float lifetime)
    {
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;

        Invoke("ReturnToPool", lifetime);
    }

    private void HandleImpact(GameObject hitObject)
    {
        DealDamage(hitObject);
        ReturnToPool();
    }

    private void DealDamage(GameObject hitObject)
    {
        if (damage <= 0) return;
        hitObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }

    private void ReturnToPool()
    {
        // Return to pool appropriately
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision) => HandleImpact(collision.gameObject);
    private void OnCollisionEnter2D(Collision2D collision) => HandleImpact(collision.gameObject);

}
