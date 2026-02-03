using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    public PowerUp powerUp;

    private void Awake() => SelectRandomPowerup();

    private void SelectRandomPowerup()
    {
        PowerUp[] powerUpValues;
        powerUpValues = (PowerUp[])System.Enum.GetValues(typeof(PowerUp));
        powerUp = powerUpValues[Random.Range(0, powerUpValues.Length)];
    }
}
