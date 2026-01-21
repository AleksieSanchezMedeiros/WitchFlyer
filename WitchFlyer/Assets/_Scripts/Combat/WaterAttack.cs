using UnityEngine;

public class WaterAttack : MonoBehaviour, IElementAttack
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject hailPrefab;
    [SerializeField] private float fireRate = 0.25f;

    private float nextFireTime;

    public void OnPressed()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        Instantiate(hailPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void OnHeld(float dt) { }
    public void OnReleased() { }
    public void OnEquipped() { }
    public void OnUnequipped() { }
}
