using UnityEngine;

public class FireAttack : MonoBehaviour, IElementAttack
{
    [Header("References")]
    [SerializeField] private Transform spawnPoint;              // where flame starts (muzzle)
    [SerializeField] private FlameProjectile flamePrefab;    // trigger hitbox (local space)

    [Header("Reach Ramp")]
    [SerializeField] private float maxReach = 5f;
    [SerializeField] private float extendSpeed = 2f;        // units per second

    [Header("Flame Stream")]
    [SerializeField] private float fireRate = 0.1f;

    [SerializeField] private int manaCost;
    [SerializeField] private bool stopWhenOutOfMana = true;

    private bool firing;
    private float currentReach;
    private float nextFireTime;
    private float manaTimer;

    public void OnPressed()
    {
        firing = true;
        currentReach = 0f;
        nextFireTime = 0f;
        manaTimer = 0f;
    }

    public void OnHeld(float dt)
    {
        if (!firing) return;

        currentReach = Mathf.Min(maxReach, currentReach + extendSpeed * dt);

        if (manaCost > 0) {
            manaTimer += dt;

            while (manaTimer > 1f) {
                manaTimer -= 1f;

                bool enoughMana = Player.Instance.UseMana(manaCost);
                if (!enoughMana) {
                    if (stopWhenOutOfMana) firing = false;
                    return;
                }
            }
        }

        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        SpawnFlameParticle(currentReach);
    }

    private void SpawnFlameParticle(float reach)
    {
        if (spawnPoint == null || flamePrefab == null) return;

        FlameProjectile projectile = Instantiate(flamePrefab, spawnPoint.position, Quaternion.identity);
        projectile.SetMaxTravelDistance(reach);
    }

    public void OnReleased() => firing = false;
    public void OnEquipped() { }
    public void OnUnequipped() => firing = false;
}
