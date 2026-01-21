using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private MonoBehaviour fireAttack;
    [SerializeField] private MonoBehaviour waterAttack;
    [SerializeField] private MonoBehaviour lightningAttack;

    private IElementAttack currentAttack;

    [Header("Element Selection")]
    public ElementList currentElement;

    private bool lastHeld;

    private void Awake()
    {
        currentElement = ElementList.Fire;
    }

    // Update is called once per frame
    void Update()
    {
        HandleElementSwitch();

        bool held = InputManager.shootPressed;
        bool pressed = held && !lastHeld;
        bool released = !held && lastHeld;

        if (pressed) currentAttack?.OnPressed();
        if (held) currentAttack?.OnHeld(Time.deltaTime);
        if (released) currentAttack?.OnReleased();
    }

    private void HandleElementSwitch()
    {
        if (InputManager.elementSwitchLeftPressed) {
            currentElement = GetLeft(currentElement);
            EquipElement(currentElement);
        }

        if (InputManager.elementSwitchRightPressed) {
            currentElement = GetRight(currentElement);
            EquipElement(currentElement);
        }
    }

    private void EquipElement(ElementList newElement)
    {
        if (currentAttack != null) currentAttack.OnUnequipped();

        currentElement = newElement;
        currentAttack = newElement switch {
            ElementList.Fire => (IElementAttack)fireAttack,
            ElementList.Water => (IElementAttack)waterAttack,
            ElementList.Lightning => (IElementAttack)lightningAttack,
            _ => currentAttack
        };

        currentAttack?.OnEquipped();
    }

    private ElementList GetLeft(ElementList current) =>
        current == ElementList.Fire ? ElementList.Water :
        current == ElementList.Water ? ElementList.Lightning :
        ElementList.Fire;

    private ElementList GetRight(ElementList current) =>
        current == ElementList.Fire ? ElementList.Lightning :
        current == ElementList.Water ? ElementList.Fire :
        ElementList.Water;
}

public enum ElementList
{
    Fire,
    Water,
    Lightning
}
