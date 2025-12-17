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
        if (InputManager.shootPressed && canShoot)
        {
            // GameObject bulletInstacne = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
            GameObject bulletInstance = ObjectPoolManager.SpawnObject(bullet, spawnPoint.transform.position,spawnPoint.transform.rotation, ObjectPoolManager.PoolType.GameObjects);
            StartCoroutine(ShootCooldown());
        }

        if (InputManager.elementSwitchLeftPressed)
        {
            ElementList nextElement = leftElement;
            leftElement = currentElement;
            currentElement = nextElement;
        }

        if (InputManager.elementSwitchRightPressed)
        {
            ElementList nextElement = rightElement;
            rightElement = currentElement;
            currentElement = nextElement;
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
