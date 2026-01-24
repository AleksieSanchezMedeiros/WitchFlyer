using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Damage IFrame")]
    [SerializeField] private float invulnerableTime = 0.5f;
    private bool isInvulnerable;

    [Header("UI")]
    [SerializeField] private TMP_Text healthDisplay;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealth();
    }

    private void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        UpdateHealth();

        if (currentHealth <= 0) Die();
        else StartCoroutine(InvulnerableCooldown());
    }

    private IEnumerator InvulnerableCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        isInvulnerable = false;
    }

    private void Die()
    {
        Destroy(gameObject);
        GameManager.Instance.GameOver();
    }

    private void UpdateHealth()
    {
        healthDisplay.text = "<color=orange>Health: </color>" + currentHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) {
            int damage = collision.gameObject.GetComponent<Enemy>().dmg;
            TakeDamage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            int damage = collision.gameObject.GetComponent<Enemy>().dmg;
            TakeDamage(damage);
        }
    }
}
