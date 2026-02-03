using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    // Queue<PowerUp> storedPowerUps = new Queue<PowerUp>();
    // We don't want multiple powers stored at a time
    [SerializeField] private CircleCollider2D playerCollider;
    [SerializeField] private PowerUp storedPowerUp;
    [SerializeField] private bool hasStoredPowerUp;

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

    [Header("PowerUp Icons")]
    [SerializeField] private Sprite healingSalveIcon;
    [SerializeField] private Sprite motePowerIcon;
    [SerializeField] private Sprite heartPowerIcon;
    [SerializeField] private Sprite stoneSkinIcon;
    [SerializeField] private Sprite songFireIcon;
    [SerializeField] private Sprite songWaterIcon;
    [SerializeField] private Sprite songLightningIcon;
    [SerializeField] private Sprite wardStoneIcon;
    [SerializeField] private Sprite grandHeartIcon;

    private void Start()
    {
        playerCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (InputManager.powerUpPressed && hasStoredPowerUp)
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
            StorePowerUp(collision.gameObject.GetComponentInParent<PowerUpDrop>().powerUp);
            // collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
    }

    public void StorePowerUp(PowerUp powerUp)
    {
        //storedPowerUps.Enqueue(powerUp);
        storedPowerUp = powerUp;
        UIManager.Instance.UpdatePowerUp(powerUp);
    }

    public void UsePowerup()
    {
        //PowerUp currentPowerUp = storedPowerUps.Dequeue();
        switch (storedPowerUp)
        {
            case PowerUp.HealingSalve:
                HealingSalve();
                break;
            case PowerUp.MoteOfPower:
                MoteOfPower();
                break;
            case PowerUp.HeartOfPower:
                HeartOfPower();
                break;
            case PowerUp.StoneSkin:
                StoneSkin();
                break;
            case PowerUp.SongOfFire:
                SongOfFire();
                break;
            case PowerUp.SongOfWater:
                SongOfWater();
                break;
            case PowerUp.SongOfLightning:
                SongOfLightning();
                break;
            case PowerUp.WardStone:
                WardStone();
                break;
            case PowerUp.GrandHeart:
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

public enum PowerUp
{
    HealingSalve,
    MoteOfPower,
    HeartOfPower,
    StoneSkin,
    SongOfFire,
    SongOfWater,
    SongOfLightning,
    WardStone,
    GrandHeart
}
