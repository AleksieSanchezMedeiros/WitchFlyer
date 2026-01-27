using NUnit.Framework;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private MonoBehaviour fireAttack;
    [SerializeField] private MonoBehaviour waterAttack;
    [SerializeField] private MonoBehaviour lightningAttack;

    private IElementAttack currentAttack;

    [Header("Element Selection")]
    public Element currentElement;

    [Header("UI")]
    [SerializeField] private TMP_Text elementDisplay;

    private void Awake()
    {
        currentElement = Element.Fire;
    }

    private void Start()
    {
        EquipElement(currentElement); // equip fire element by default; REMOVE for prototype
    }

    private void OnDisable()
    {
        Debug.LogError($"DISABLED: {name} ({GetType().Name})\n{Environment.StackTrace}");
    }

    // Update is called once per frame
    void Update()
    {
        HandleElementSwitch();

        bool held = InputManager.shootHeld;
        bool pressed = InputManager.shootPressed;
        bool released = InputManager.shootReleased;

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

    private void EquipElement(Element newElement)
    {
        if (currentAttack != null) currentAttack.OnUnequipped();

        currentElement = newElement;
        currentAttack = newElement switch {
            Element.Fire => (IElementAttack)fireAttack,
            Element.Water => (IElementAttack)waterAttack,
            Element.Lightning => (IElementAttack)lightningAttack,
            _ => currentAttack
        };

        currentAttack?.OnEquipped();
        UIManager.Instance.UpdateElement(currentElement);
    }

    private Element GetLeft(Element current) =>
        current == Element.Fire ? Element.Water :
        current == Element.Water ? Element.Lightning :
        Element.Fire;

    private Element GetRight(Element current) =>
        current == Element.Fire ? Element.Lightning :
        current == Element.Water ? Element.Fire :
        Element.Water;
}

public enum Element
{
    Fire,
    Water,
    Lightning
}
