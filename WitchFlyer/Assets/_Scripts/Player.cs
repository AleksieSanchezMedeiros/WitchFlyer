using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth;

    [Header("Mana")]
    [SerializeField] private int maxMana = 100;
    [SerializeField] private int currentMana;
    [Space(10)]
    [SerializeField] private bool passiveManaRegenEnabled = true;
    [SerializeField] private float passiveManaRegenDelay = 1f;
    [SerializeField] private float passiveManaRegenRate = 0.5f;
    [SerializeField] private int passiveManaRegenPerTick = 1;
    private Coroutine passiveManaRegenCoroutine;

    [Header("PowerUps")]
    private Coroutine heartPowercoroutine;
    private float heartPowerRemaining;
    [SerializeField] private bool dontUseMana; // Heart of Power

    private Coroutine stoneSkinCoroutine;
    private float stoneSkinRemaining;
    private int stoneSkinReduction;
    [SerializeField] private bool stoneSkin; // Stone Skin

    private Element songElement;
    private int songBonus;
    private float songRemaining;
    private Coroutine songCoroutine;

    private Coroutine wardStoneCoroutine;
    private float wardStoneRemaining;
    private int wardStoneDamage;
    [SerializeField] private bool wardStone;

    [Header("Damage IFrame")]
    [SerializeField] private float invulnerableTime = 0.5f;
    private bool isInvulnerable;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        PlayerShooting.OnShootingStart += HandleShootingStart;
        PlayerShooting.OnShootingStop += HandleShootingEnd;
    }

    private void OnDisable()
    {
        PlayerShooting.OnShootingStart -= HandleShootingStart;
        PlayerShooting.OnShootingStop -= HandleShootingEnd;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentHealth = maxHealth;
        UIManager.Instance.SetMaxHealth(maxHealth);
        UIManager.Instance.UpdateHealth(currentHealth);

        currentMana = maxMana;
        UIManager.Instance.SetMaxMana(maxMana);
        UIManager.Instance.UpdateMana(currentMana);
    }

    private void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        if (stoneSkin) {
            currentHealth -= (Mathf.Max(0, damage - stoneSkinReduction));
        }else {
            currentHealth -= damage;
        }
        UIManager.Instance.UpdateHealth(currentHealth);

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

    #region RESOURCES
    public void RegenHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UIManager.Instance.UpdateHealth(currentHealth);
    }

    public void RegenMana(int amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        UIManager.Instance.UpdateMana(currentMana);
    }

    public bool UseMana(int manaCost)
    {
        if (manaCost > currentMana) return false;

        if (!dontUseMana) {
            currentMana -= manaCost;
            UIManager.Instance.UpdateMana(currentMana);
        }

        return true;
    }

    // Mana Regeneration
    private void HandleShootingStart()
    {
        Debug.Log("STOP MANA REGENERATION!");
        // Stop regeneration
        if (passiveManaRegenCoroutine != null) {
            StopCoroutine(passiveManaRegenCoroutine);
            passiveManaRegenCoroutine = null;
        }
    }

    private void HandleShootingEnd()
    {
        Debug.Log("CAN START MANA REGENRATION");
        if (!passiveManaRegenEnabled) return;

        if (passiveManaRegenCoroutine != null) StopCoroutine(passiveManaRegenCoroutine);
        passiveManaRegenCoroutine = StartCoroutine(PassiveManaRegeneration());
    }

    private IEnumerator PassiveManaRegeneration()
    {
        yield return new WaitForSeconds(passiveManaRegenDelay);

        while (currentMana < maxMana) {
            RegenMana(passiveManaRegenPerTick);
            yield return new WaitForSeconds(passiveManaRegenRate);
        }

        passiveManaRegenCoroutine = null;
    }


    #endregion

    #region POWERUPS

    // Heart of Power
    public void HeartOfPower(float duration)
    {
        heartPowerRemaining += (heartPowerRemaining <= 0f) ? duration : duration / 2f;
        if (heartPowercoroutine == null) heartPowercoroutine = StartCoroutine(HeartOfPowerRoutine());
    }

    private IEnumerator HeartOfPowerRoutine()
    {
        dontUseMana = true;

        while (heartPowerRemaining > 0f) {
            heartPowerRemaining -= Time.deltaTime;
            yield return null;
        }

        dontUseMana = false;
        heartPowercoroutine = null;
    }

    // Stone Skin
    public void StoneSkin(float duration, int reductionValue)
    {
        UpdateStoneSkinDamageReduction(reductionValue);

        stoneSkinRemaining += (stoneSkinRemaining <= 0f) ? duration : duration / 2f;
        if (stoneSkinCoroutine == null) stoneSkinCoroutine = StartCoroutine(StoneSkinCoroutine());
    }

    private IEnumerator StoneSkinCoroutine()
    {
        stoneSkin = true;

        while (stoneSkinRemaining > 0f) {
            stoneSkinRemaining -= Time.deltaTime;
            yield return null;
        }

        stoneSkin = false;
        stoneSkinCoroutine = null;
    }

    // Use the higher value of the current and new values
    // Kinda goofy logistically
    private void UpdateStoneSkinDamageReduction(int value) => stoneSkinReduction = Mathf.Max(stoneSkinReduction, value);


    // Songs of Element
    public void ActivateSong(Element element, int bonusDamage, float duration)
    {
        if (songCoroutine != null && songElement == element) songRemaining += duration / 2f;
        else songRemaining = duration;

        songElement = element;
        songBonus = bonusDamage;

        songRemaining = duration;

        if (songCoroutine == null) songCoroutine = StartCoroutine(SongCoroutine());
    }

    private IEnumerator SongCoroutine()
    {
        while (songRemaining > 0f) {
            songRemaining -= Time.deltaTime;
            yield return null;
        }
        songBonus = 0;
        songCoroutine = null;
    }

    public int GetSongBonus(Element element)
    {
        return (songRemaining > 0f && element == songElement) ? songBonus : 0;
    }

    // Ward Stone
    public void WardStone(float duration, int damageValue)
    {
        UpdateWardStoneDamageReduction(damageValue);

        wardStoneRemaining += (wardStoneRemaining <= 0f) ? duration : duration / 2f;
        if (wardStoneCoroutine == null) wardStoneCoroutine = StartCoroutine(WardStoneCoroutine());
    }

    private IEnumerator WardStoneCoroutine()
    {
        wardStone = true;

        while (wardStoneRemaining > 0f) {
            wardStoneRemaining -= Time.deltaTime;
            yield return null;
        }

        wardStone = false;
        wardStoneCoroutine = null;
    }

    private void UpdateWardStoneDamageReduction(int value) => wardStoneDamage = value;


    // Grand Heart
    public void GrandHeart(int maxGain, int healAmount, int healthCap, int cappedHeal)
    {
        if (maxHealth >= healthCap) {
            RegenHealth(cappedHeal);
        }else {
            maxHealth = Mathf.Min(maxHealth + maxGain, healthCap);
            RegenHealth(healAmount);
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            TakeDamage(enemy.damage);

            if (wardStone) enemy.TakeDamage(wardStoneDamage);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy") {
    //        int damage = collision.gameObject.GetComponent<Enemy>().dmg;
    //        TakeDamage(damage);
    //    }
    //}
}
