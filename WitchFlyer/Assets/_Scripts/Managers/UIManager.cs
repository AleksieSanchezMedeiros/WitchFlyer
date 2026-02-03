using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject gameOverPanel;

    [Header("HUD")]
    [Header("Health")]
    [SerializeField] private TMP_Text healthDisplay;
    [SerializeField] private Transform heartsContainer;
    [SerializeField] private Image heartPrefab;
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartEmpty;
    private Image[] hearts;

    [Header("Mana")]
    [SerializeField] private TMP_Text manaDisplay;
    [SerializeField] private Image manaFillImage;
    private int maxMana;

    [Header("Element")]
    [SerializeField] private TMP_Text elementDisplay;
    [SerializeField] private LayoutElement fireIcon;
    [SerializeField] private LayoutElement waterIcon;
    [SerializeField] private LayoutElement lightningIcon;
    [SerializeField] private Vector2 baseSize;
    [SerializeField] private Vector2 largeSize;

    [Header("PowerUps")]
    [SerializeField] private TMP_Text powerUpDisplay;
    [SerializeField] private Image powerUpIcon;

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

    #region HEALTH
    public void UpdateHealth(int healthValue)
    {
        healthDisplay.text = healthValue.ToString();

        if (hearts == null || hearts.Length == 0) GenerateHearts(healthValue);
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = (i < healthValue) ? heartFull : heartEmpty;
        }
    }

    private void GenerateHearts(int maxHealth)
    {
        if (heartsContainer == null || heartPrefab == null) return;

        foreach (Transform child in heartsContainer)
            Destroy(child.gameObject);

        hearts = new Image[maxHealth];

        for (int i = 0; i < maxHealth; i++) {
            Image img = Instantiate(heartPrefab, heartsContainer);
            img.sprite = heartEmpty;
            hearts[i] = img;
        }
    }

    public void SetMaxHealth(int maxHealth) => GenerateHearts(maxHealth);
    #endregion

    #region MANA
    public void UpdateMana(int manaValue)
    {
        manaDisplay.text = manaValue.ToString();

        float progress = (maxMana <= 0) ? 0f : (float)manaValue / maxMana;
        manaFillImage.fillAmount = progress;
    }

    public void SetMaxMana(int maxMana) => this.maxMana = maxMana;
    #endregion

    #region ELEMENT
    public void UpdateElement(Element element)
    {
        SetPreferredSize(fireIcon, false);
        SetPreferredSize(waterIcon, false);
        SetPreferredSize(lightningIcon, false);

        switch (element) {
            case Element.Fire:
                elementDisplay.text = Element.Fire.ToString();
                SetPreferredSize(fireIcon, true);
                break;
            case Element.Water:
                elementDisplay.text = Element.Water.ToString();
                SetPreferredSize(waterIcon, true);
                break;
            case Element.Lightning:
                elementDisplay.text = Element.Lightning.ToString();
                SetPreferredSize(lightningIcon, true);
                break;
        }
    }

    private void SetPreferredSize(LayoutElement icon, bool enlarge)
    {
        if (enlarge) {
            icon.preferredWidth = largeSize.x;
            icon.preferredHeight = largeSize.y;
        }else {
            icon.preferredWidth = baseSize.x;
            icon.preferredHeight = baseSize.y;
        }
    }
    #endregion

    #region POWERUPS
    public void UpdatePowerUp(PowerUp powerUp)
    {
        // Interesting way to split strings by capital letters
        string addedSpaces = Regex.Replace(powerUp.ToString(), "(?<!^)([A-Z])", " $1");
        powerUpDisplay.text = addedSpaces;
    }

    #endregion

    private void GameOver(GameState oldState, GameState newState)
    {
        if (newState == GameState.GameOver) {
            gameOverPanel.SetActive(true);
        }
    }
}
