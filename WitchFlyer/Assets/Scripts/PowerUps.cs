using System.Collections.Generic;
using UnityEngine;

public enum PowerUpsList
{
    HealingSalve,
    MotePower,
    HeartPower,
    StoneSkin,
    SongFire,
    SongWater,
    SongLightning,
    WardStone,
    GrandHeart
}

public class PowerUps : MonoBehaviour
{
    [SerializeField] private CircleCollider2D playerCollider;
    Queue<PowerUpsList> storedPowerUps = new Queue<PowerUpsList>();

    private void Start()
    {
        playerCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (InputManager.powerUpPressed && storedPowerUps.Count > 0)
        {
            // storedPowerUps.Dequeue()
            UsePower();
            Debug.Log("Powe Up used");
        }
        else
        {
            // Display message saying no power ups
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            AddPower(collision.gameObject.GetComponent<PowerUpDrop>().powerUp);
            collision.gameObject.SetActive(false);
        }
    }

    public void AddPower(PowerUpsList powerUp)
    {
        storedPowerUps.Enqueue(powerUp);
    }

    public void UsePower()
    {
        PowerUpsList currentPowerUp = storedPowerUps.Dequeue();
        switch (currentPowerUp)
        {
            case PowerUpsList.HealingSalve:
                break;
            case PowerUpsList.MotePower:
                break;
            case PowerUpsList.HeartPower:
                break;
            case PowerUpsList.StoneSkin:
                break;
            case PowerUpsList.SongFire:
                break;
            case PowerUpsList.SongWater:
                break;
            case PowerUpsList.SongLightning:
                break;
            case PowerUpsList.WardStone:
                break;
            case PowerUpsList.GrandHeart:
                break;
        }
    }
}
