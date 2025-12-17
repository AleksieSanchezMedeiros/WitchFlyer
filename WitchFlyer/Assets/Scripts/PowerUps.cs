using System.Collections.Generic;
using UnityEngine;

public enum PowerUpsList
{

}

public class PowerUps : MonoBehaviour
{

    Queue<PowerUpsList> storedPowerUps = new Queue<PowerUpsList>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.powerUpPressed && storedPowerUps.Count > 0)
        {
            // storedPowerUps.Dequeue()
            Debug.Log("Powe Up used");
        }
    }
}
