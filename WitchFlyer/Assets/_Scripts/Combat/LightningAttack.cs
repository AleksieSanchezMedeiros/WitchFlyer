using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    [SerializeField] private Transform origin;
    [SerializeField] private float minCharge = 0.1f;
    [SerializeField] private float maxCharge = 2f;

    [Header("Scaling")]
    [SerializeField] private int baseDamage = 1;
    [SerializeField] private int maxBonusDamage = 2;
    [SerializeField] private int baseChain = 2;
    [SerializeField] private int maxBonusChain = 5;

    private float chargeTime;
    private bool charging;

    public void OnPressed()
    {
        charging = true;
        chargeTime = 0f;
    }

    public void OnReleased()
    {
        if (!charging) return;
        charging = false;

        if (chargeTime < minCharge) return;

        float t = Mathf.InverseLerp(minCharge, maxCharge, chargeTime);
        int damage = baseDamage + Mathf.RoundToInt(maxBonusDamage * t);
        int chain = baseChain + Mathf.RoundToInt(maxBonusChain * t);

        ReleaseLightning(damage, chain);
    }

    private void ReleaseLightning(int damage, int chainLength)
    {
        // CHAIN ATTACK
        Debug.Log("CHAIN LIGHTNING ZAPPY ZAP!");
    }

    public void OnEquipped() { }
    public void OnUnequipped() { charging = false; }
}