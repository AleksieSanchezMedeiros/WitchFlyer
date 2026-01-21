using UnityEngine;

public class FireAttack : MonoBehaviour
{
    [SerializeField] private GameObject flamePrefab;
    [SerializeField] private float tickInterval = 0.1f;

    private float tickTimer;

    public void OnPressed()
    {
        flamePrefab.SetActive(true);
        tickTimer = 0f;
    }

    public void OnHeld(float dt)
    {
        tickTimer -= dt;
        if (tickTimer <= 0f) tickTimer = tickInterval;
    }

    public void OnReleased()
    {
        flamePrefab.SetActive(false);
    }

    public void OnEquipped() { }
    public void OnUnequipped() { flamePrefab.SetActive(false); }
}
