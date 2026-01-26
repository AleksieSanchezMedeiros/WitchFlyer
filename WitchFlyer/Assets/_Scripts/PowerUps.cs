using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private CircleCollider2D playerCollider;
    Queue<PowerUpsList> storedPowerUps = new Queue<PowerUpsList>();

    [Header("PowerUp Stats")]
    [SerializeField] private int healingSalve;
    [SerializeField] private int motePower;
    [SerializeField] private float heartPowerDuration;
    [SerializeField] private float stoneSkinDuration;
    [SerializeField] private int stoneSkinReduction;
    [SerializeField] private int songFireBonus;
    [SerializeField] private int songWaterBonus;
    [SerializeField] private int songLightningBonus;
    [SerializeField] private float songDuration;
    [SerializeField] private float wardStoneDuration;
    [SerializeField] private int wardStoneDamage;
    [Space(10)]
    [SerializeField] private int grandHeartMaxGain; // Max health increase amount
    [SerializeField] private int grandHeartHeal; // Heal amount
    [SerializeField] private int grandHeartHealthCap; // What is the health cap
    [SerializeField] private int grandHeartCapHeal; // How much it heals if player is at the health cap

    private void Start()
    {
        playerCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (InputManager.powerUpPressed && storedPowerUps.Count > 0)
        {
            // storedPowerUps.Dequeue()
            UsePowerup();
            Debug.Log("Power Up Used!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            AddPower(collision.gameObject.GetComponentInParent<PowerUpDrop>().powerUp);
            // collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
    }

    public void AddPower(PowerUpsList powerUp)
    {
        storedPowerUps.Enqueue(powerUp);
    }

    public void UsePowerup()
    {
        PowerUpsList currentPowerUp = storedPowerUps.Dequeue();
        switch (currentPowerUp)
        {
            case PowerUpsList.HealingSalve:
                HealingSalve();
                break;
            case PowerUpsList.MotePower:
                MoteOfPower();
                break;
            case PowerUpsList.HeartPower:
                HeartOfPower();
                break;
            case PowerUpsList.StoneSkin:
                StoneSkin();
                break;
            case PowerUpsList.SongFire:
                SongOfFire();
                break;
            case PowerUpsList.SongWater:
                SongOfWater();
                break;
            case PowerUpsList.SongLightning:
                SongOfLightning();
                break;
            case PowerUpsList.WardStone:
                WardStone();
                break;
            case PowerUpsList.GrandHeart:
                GrandHeart();
                break;
        }
    }

    // Heals the player for x half hearts
    private void HealingSalve()
    {
        Player.Instance.RegenHealth(healingSalve); // HP poin = half heart
    }

    // Fills the magic gauge for x points
    private void MoteOfPower()
    {
        Player.Instance.RegenMana(motePower);
    }

    // For the duration, the magic gauge doesn’t decrease.
    private void HeartOfPower()
    {
        Player.Instance.HeartOfPower(heartPowerDuration);
    }

    // For the duration damage received is reduced by 1 to a minimum of 0.
    private void StoneSkin()
    {
        Player.Instance.StoneSkin(stoneSkinDuration, stoneSkinReduction);
    }

    // Elemental attacks of fire increase their damage by 1.
    private void SongOfFire()
    {
        Player.Instance.ActivateSong(Element.Fire, songFireBonus, songDuration);
    }

    // Elemental attacks of water increase their damage by 1.
    private void SongOfWater()
    {
        Player.Instance.ActivateSong(Element.Water, songWaterBonus, songDuration);
    }

    // Elemental attacks of lightning increase their damage by 1.
    private void SongOfLightning()
    {
        Player.Instance.ActivateSong(Element.Lightning, songLightningBonus, songDuration);
    }

    private void WardStone()
    {
        Player.Instance.WardStone(wardStoneDuration, wardStoneDamage);
    }

    private void GrandHeart()
    {
        Player.Instance.GrandHeart(grandHeartMaxGain, grandHeartHeal, grandHeartHealthCap, grandHeartCapHeal);
    }
}

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
