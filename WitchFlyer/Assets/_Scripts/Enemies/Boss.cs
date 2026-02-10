using System;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private GameObject elementalBallPrefab;
    [SerializeField] private GameObject rusherBatPrefab;

    [SerializeField] private GameObject attack1Prefab;
    [SerializeField] private GameObject attack2Prefab;
    [SerializeField] private GameObject utility1Prefab;
    [SerializeField] private GameObject utility2Prefab;

    [Header("Boss Positioning")]
    [SerializeField] private Transform bossParent;
    [SerializeField] private Transform spawnPos;

    [Header("Movement")]
    [SerializeField] private BossMoveMode moveMode = BossMoveMode.Random;
    [SerializeField] private float moveRange = 3.5f;
    [SerializeField] private float moveSpeed = 3f;
    [Space(10)]
    [SerializeField] private float swayAmplitude = 0.5f;
    [SerializeField] private float swayFrequency = 0.5f;

    [Header("Targeting")]
    [SerializeField] private float retargetInterval = 1f;
    [SerializeField] private float trackOffset = 0f;

    [Header("Attacks")]
    [SerializeField] private float attack1Cooldown;
    [SerializeField] private float attack2Cooldown;
    [SerializeField] private float utility1Cooldown;
    [SerializeField] private float utility2Cooldown;
    private float nextAttack1Time;
    private float nextAttack2Time;
    private float nextUtility1Time;
    private float nextUtility2Time;

    private Transform player;

    private float spawnTime;
    private float retargetTime;
    private float targetPos;

    public static Action OnBossDeath;

    private void Start()
    {
        player = Player.Instance.transform;

        if (bossParent == null) Debug.LogError("Boss parent NOT set!");

        targetPos = Mathf.Clamp(transform.localPosition.y, -moveRange, moveRange);
        retargetTime = Time.time + retargetInterval;
    }

    public override void Move()
    {
        if (bossParent == null) return;

        if (moveMode == BossMoveMode.TrackPlayer && player != null) {
            targetPos = Mathf.Clamp(player.position.y + trackOffset, -moveRange, moveRange);
        }else {
            if (Time.time >= retargetTime) {
                targetPos = UnityEngine.Random.Range(-moveRange, moveRange);
                retargetTime = Time.time + retargetInterval;
            }
        }

        float newPos = Mathf.MoveTowards(transform.localPosition.y, targetPos, moveSpeed * Time.deltaTime);
        float time = Time.time - spawnTime;
        float sway = Mathf.Sin(time * Mathf.PI * 2f * swayFrequency) * swayAmplitude;

        transform.localPosition = new Vector3(bossParent.localPosition.x - Mathf.Abs(sway), newPos, bossParent.localPosition.z);
    }

    public override void Attack()
    {
        if (health <= 0) return;

        // Attack 1
        if (attack1Prefab != null && Time.time >= nextAttack1Time) {
            SpawnAttackOrUtility(attack1Prefab);
            nextAttack1Time = Time.time + attack1Cooldown;
        }

        if (attack2Prefab != null && Time.time >= nextAttack2Time) {
            SpawnAttackOrUtility(attack2Prefab);
            nextAttack2Time = Time.time + attack2Cooldown;
        }

        if (Time.time >= nextUtility1Time) {
            SpawnAttackOrUtility(utility1Prefab);
            nextUtility1Time = Time.time + utility1Cooldown;
        }

        if (Time.time >= nextUtility2Time) {
            SpawnAttackOrUtility(utility2Prefab);
            nextUtility2Time = Time.time + utility2Cooldown;
        }
    }

    private void SpawnAttackOrUtility(GameObject prefab) {
        Instantiate(prefab, spawnPos.position, Quaternion.identity);
    }

    public override void CheckHealth()
    {
        if (health <= 0) {
            OnBossDeath?.Invoke();
            DropPowerUp();

            Destroy(gameObject);
        }
    }

    private void SummonRusherBat()
    {

    }

    private void SpawnObstacle()
    {

    }
}

public enum BossMoveMode
{
    Random,
    TrackPlayer
}
