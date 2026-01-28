using UnityEngine;

public class FlameProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float startSpeed = 4f;
    [SerializeField] private float drag = 6f;              
    [SerializeField] private float lifetime = 0.8f;      

    [Header("Reach")]
    [SerializeField] private float maxTravelDistance = 3f;
    [SerializeField] private float flameConeDegree = 5f;

    [Header("Damage")]
    [SerializeField] private int baseDamage = 1;

    [Header("Fade (optional)")]
    [SerializeField] private bool fadeOut = true;
    [SerializeField] private float fadeLastPercent = 0.4f; 

    private float lifeTimer;
    private float currentSpeed;
    private Vector2 spawnPos;

    private SpriteRenderer spriteRenderer;
    private float startAlpha = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null) startAlpha = spriteRenderer.color.a;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.isTrigger = true;
    }

    private void OnEnable()
    {
        lifeTimer = lifetime;
        currentSpeed = startSpeed;
        spawnPos = transform.position;

        if (spriteRenderer != null) {
            Color color = spriteRenderer.color;
            color.a = startAlpha;
            spriteRenderer.color = color;
        }
    }

    private void Start()
    {
        transform.right = GetMoveDirection();
    }

    public void SetMaxTravelDistance(float distance) => maxTravelDistance = Mathf.Max(0.1f, distance);

    private void Update()
    {
        transform.position += transform.right * (currentSpeed * Time.deltaTime);
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, drag * Time.deltaTime);

        if (Vector2.Distance(spawnPos, transform.position) >= maxTravelDistance) {
            Destroy(gameObject);
            return;
        }

        lifeTimer -= Time.deltaTime;

        if (fadeOut && spriteRenderer != null) {
            float t = 1f - Mathf.Clamp01(lifeTimer / lifetime);
            float fadeStart = 1f - Mathf.Clamp01(fadeLastPercent);

            if (t >= fadeStart) {
                float u = Mathf.InverseLerp(fadeStart, 1f, t);
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(startAlpha, 0f, u);
                spriteRenderer.color = color;
            }
        }

        if (lifeTimer <= 0f) {
            Destroy(gameObject);
        }
    }

    private Vector3 GetMoveDirection()
    {
        spawnPos = transform.position;

        Vector2 playerPos = Player.Instance.transform.position;
        Vector2 direction = spawnPos - playerPos;

        if (direction.sqrMagnitude < 0.0001f) direction = Vector2.right;
        direction.Normalize();

        float spread = Random.Range(-flameConeDegree, flameConeDegree);
        direction = (Quaternion.Euler(0, 0, spread) * direction).normalized;

        return direction;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null) return;

        enemy.TryTakeFireDamage(baseDamage);
    }
}
