using UnityEngine;

public class FireAttack : MonoBehaviour, IElementAttack
{
    [Header("References")]
    [SerializeField] private Transform origin;              // where flame starts (muzzle)
    [SerializeField] private Transform flameVisual;         // visual object to rotate/scale
    [SerializeField] private BoxCollider2D flameHitbox;     // trigger hitbox (local space)

    [Header("Reach Ramp")]
    [SerializeField] private float maxLength = 5f;
    [SerializeField] private float extendSpeed = 8f;        // units per second

    [Header("Damage Ticks")]
    [SerializeField] private float tickInterval = 0.1f;
    [SerializeField] private int tickDamage = 1;
    [SerializeField] private LayerMask enemyMask;

    private float currentLength;
    private float tickTimer;
    private bool firing;

    public void OnPressed()
    {
        firing = true;
        currentLength = 0f;
        tickTimer = 0f;

        flameVisual.gameObject.SetActive(true);
        flameHitbox.enabled = true;
    }

    public void OnHeld(float dt)
    {
        if (!firing) return;

        // 1) Ramp length while held
        currentLength = Mathf.Min(maxLength, currentLength + extendSpeed * dt);

        // 2) Aim direction from origin to mouse (world)
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = origin.position.z;

        Vector2 dir = (mouseWorld - origin.position);
        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;
        dir.Normalize();

        // 3) Position & rotate flame to aim direction (assumes flame points +X locally)
        flameVisual.position = origin.position;
        flameVisual.right = dir;

        // 4) Scale visual and hitbox to match currentLength
        // Visual: scale X to length (keep Y as-is)
        Vector3 s = flameVisual.localScale;
        flameVisual.localScale = new Vector3(currentLength, s.y, s.z);

        // Hitbox: start at origin and extend forward
        flameHitbox.size = new Vector2(currentLength, flameHitbox.size.y);
        flameHitbox.offset = new Vector2(currentLength * 0.5f, 0f);

        // 5) Damage tick using overlap box aligned to aim
        tickTimer -= dt;
        if (tickTimer <= 0f) {
            tickTimer = tickInterval;

            Vector2 center = (Vector2)origin.position + dir * (currentLength * 0.5f);
            Vector2 size = new Vector2(currentLength, flameHitbox.size.y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            var hits = Physics2D.OverlapBoxAll(center, size, angle, enemyMask);
            for (int i = 0; i < hits.Length; i++) {
                var enemy = hits[i].GetComponentInParent<Enemy>();
                if (enemy != null) enemy.TakeDamage(tickDamage);
            }
        }
    }

    public void OnReleased()
    {
        firing = false;
        flameVisual.gameObject.SetActive(false);
        flameHitbox.enabled = false;
    }

    public void OnEquipped() { }
    public void OnUnequipped() { OnReleased(); }
}
