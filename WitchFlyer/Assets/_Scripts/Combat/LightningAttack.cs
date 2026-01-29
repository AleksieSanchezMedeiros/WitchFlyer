using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour, IElementAttack
{
    [SerializeField] private Transform origin;
    [SerializeField] private LightningProjectile lightningPrefab;
    [SerializeField] private float minCharge = 0.1f;
    [SerializeField] private float maxCharge = 2f;
    [SerializeField] private float currentCharge;

    [Header("Scaling")]
    [SerializeField] private int baseDamage = 1;
    [SerializeField] private int maxBonusDamage = 2;
    [SerializeField] private int baseChain = 2;
    [SerializeField] private int maxBonusChain = 5;
    [SerializeField] private int baseManaCost = 4;
    [SerializeField] private int maxBonusManaCost = 4;
    
    private float chargeTime;
    private bool charging;

    public void OnPressed()
    {
        Debug.Log("LIGHTNING PRESS");
        charging = true;
        chargeTime = 0f;
    }

    public void OnHeld(float dt)
    {
        if (!charging) return;
        chargeTime += dt;
        chargeTime = Mathf.Min(chargeTime, maxCharge);
    }

    public void OnReleased()
    {
        Debug.Log("LIGHTNING RELEASE");
        if (!charging) return;
        if (chargeTime < minCharge) return; // Can't just tap to fire

        float t = Mathf.InverseLerp(minCharge, maxCharge, chargeTime);
        int damage = baseDamage + Mathf.RoundToInt(maxBonusDamage * t);
        int chainLength = baseChain + Mathf.RoundToInt(maxBonusChain * t);
        int manaCost = baseManaCost + Mathf.RoundToInt(maxBonusManaCost * t);

        bool enoughMana = Player.Instance.UseMana(manaCost);
        if (!enoughMana) return;

        charging = false;
        ReleaseLightning(damage, chainLength);
    }

    private void Update()
    {
        currentCharge = chargeTime;
    }

    private void ReleaseLightning(int damage, int chainLength)
    {
        if (lightningPrefab == null || origin == null) return;

        HashSet<int> hitIds = new HashSet<int>();
        LightningProjectile projectile = Instantiate(lightningPrefab, origin.position, Quaternion.identity);
        projectile.Initialize(origin.right, damage, chainLength, hitIds);
    }

    public void OnEquipped() { }
    public void OnUnequipped() { charging = false; }
}