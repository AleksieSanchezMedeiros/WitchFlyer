using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    public PowerUpsList powerUp;

    private void Awake() => SelectRandomPowerup();

    private void SelectRandomPowerup()
    {
        PowerUpsList[] powerUpValues;
        powerUpValues = (PowerUpsList[])System.Enum.GetValues(typeof(PowerUpsList));
        powerUp = powerUpValues[Random.Range(0, powerUpValues.Length)];
    }
}
