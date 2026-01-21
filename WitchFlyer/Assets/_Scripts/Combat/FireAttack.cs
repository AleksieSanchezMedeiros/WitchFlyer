using UnityEngine;

public class FireAttack : MonoBehaviour, IElementAttack
{
    [SerializeField] private GameObject flamePrefab;
    [SerializeField] private float tickInterval = 0.1f;

    private float tickTimer;

    public void OnPressed()
    {
        Debug.Log("FIRE PRESS");
        flamePrefab.SetActive(true);
        tickTimer = 0f;
    }

    public void OnHeld(float dt)
    {
        Debug.Log("FIRE HOLD");
        tickTimer -= dt;
        if (tickTimer <= 0f) tickTimer = tickInterval;
    }

    public void OnReleased()
    {
        Debug.Log("FIRE RELEASE");
        flamePrefab.SetActive(false);
    }

    public void OnEquipped() { }
    public void OnUnequipped() { flamePrefab.SetActive(false); }
}
