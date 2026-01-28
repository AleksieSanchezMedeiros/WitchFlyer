using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject gameOverPanel;

    [Header("HUD")]
    [SerializeField] private TMP_Text healthDisplay;
    [SerializeField] private TMP_Text manaDisplay;
    [SerializeField] private TMP_Text elementDisplay;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameOver;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameOver;
    }

    public void UpdateHealth(int healthValue)
    {
        healthDisplay.text = "<color=orange>Health: </color>" + healthValue;
    }

    public void UpdateMana(int manaValue)
    {
        manaDisplay.text = "<color=blue>Mana: </color>" + manaValue;
    }

    public void UpdateElement(Element element)
    {
        switch (element) {
            case Element.Fire:
                elementDisplay.text = "<color=orange>Element: </color>" + Element.Fire.ToString();
                break;
            case Element.Water:
                elementDisplay.text = "<color=blue>Element: </color>" + Element.Water.ToString();
                break;
            case Element.Lightning:
                elementDisplay.text = "<color=yellow>Element: </color>" + Element.Lightning.ToString();
                break;
        }
    }

    private void GameOver(GameState oldState, GameState newState)
    {
        Debug.Log("HERE");
        if (newState == GameState.GameOver) {
            Debug.Log("GAME OVER");
            gameOverPanel.SetActive(true);
        }
    }
}
