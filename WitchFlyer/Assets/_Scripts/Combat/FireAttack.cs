using UnityEngine;

public class FireAttack : MonoBehaviour, IElementAttack
{
    [Header("References")]
    [SerializeField] private Transform origin;              // where flame starts (muzzle)
    [SerializeField] private GameObject fireProjectile;    // trigger hitbox (local space)

    [Header("Reach Ramp")]
    [SerializeField] private float maxReach = 5f;
    [SerializeField] private float extendSpeed = 2f;        // units per second

    [Header("Flame Stream")]
    [SerializeField] private float tickInterval = 0.1f;
    [SerializeField] private float flameConeDegree;

    [SerializeField] private int manaCost;

    private float currentReach;
    private float fireTimer;
    private float manaTimer;
    private bool firing;

    public void OnPressed()
    {
        firing = true;
        currentReach = 0f;
        fireTimer = 0f;
        manaTimer = 0f;
    }

    public void OnHeld(float dt)
    {
        if (!firing) return;

        currentReach = Mathf.Min(maxReach, currentReach + extendSpeed * dt);

        if (manaCost > 0) {
            manaTimer += dt;
            // Per Second Tick
            if (manaTimer > 1f) {
                manaTimer -= 1f;
                if (!Player.Instance.UseMana(manaCost)) {
                    firing = false;
                    return;
                }
            }
        }

        fireTimer -= dt;
        while (fireTimer <= 0f) {
            fireTimer = tickInterval;
            SpawnFlameParticle(currentReach);
        }
    }

    public void OnReleased()
    {
        //firing = false;
        //flameVisual.gameObject.SetActive(false);
        //flameHitbox.enabled = false;
    }


    private void SpawnFlameParticle(float reach)
    {
        if (origin == null || fireProjectile == null) return;

        Vector3 mouseWorld
    }

    public void OnEquipped() { }
    public void OnUnequipped() { OnReleased(); }
}
