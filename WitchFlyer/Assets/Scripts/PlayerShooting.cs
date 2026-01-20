using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;

public enum ElementList
{
    Fire,
    Water,
    Lightning
}

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject spawnPoint;
    [Header("Water Element")]
    [SerializeField] private GameObject hailProjectilePrefab;
    [SerializeField] private float hailFireRate = 0.5f;
    private float nextFireTime;

    [Header("Lightning Element")]
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private float lightningCooldown;
    [SerializeField] private float chargeLimit;

    [Header("Element Selection")]
    public ElementList currentElement;
    public ElementList leftElement;
    public ElementList rightElement;

    private bool canShoot = true;
    public float shootCooldown = 3f;

    private void Awake()
    {
        currentElement = ElementList.Fire;
        leftElement = ElementList.Water;
        rightElement = ElementList.Lightning;
    }

    // Update is called once per frame
    void Update()
    {
        HandleElementSwitch();

        if (InputManager.shootPressed && canShoot) {

            switch (currentElement) {
                case ElementList.Fire:
                    //FireAttack();
                    break;
                case ElementList.Water:
                    StartCoroutine(ShootCooldown());
                    //IceAttack();
                    break;
                case ElementList.Lightning:
                    //LightningAttack();
                    break;
            }
        }
    }

    private void HandleElementSwitch()
    {
        if (InputManager.elementSwitchLeftPressed) {
            currentElement = GetLeft(currentElement);
        }

        if (InputManager.elementSwitchRightPressed) {
            currentElement = GetRight(currentElement);
        }
    }


    private ElementList GetLeft(ElementList current) =>
        current == ElementList.Fire ? ElementList.Water :
        current == ElementList.Water ? ElementList.Lightning :
        ElementList.Fire;

    private ElementList GetRight(ElementList current) =>
        current == ElementList.Fire ? ElementList.Lightning :
        current == ElementList.Water ? ElementList.Fire :
        ElementList.Water;

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
