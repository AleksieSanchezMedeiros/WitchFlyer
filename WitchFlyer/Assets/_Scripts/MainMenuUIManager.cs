using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }else {
            Destroy(gameObject);
        }
    }

    [SerializeField] private Button playButton;
    [SerializeField] private Button guideButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button yesQuitButton;
    [SerializeField] private Button noQuitButton;

    [SerializeField] private GameObject guidePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject quitConfirmationPanel;

    private void Start() {
        playButton.onClick.AddListener(OnPlayButtonClick);
        guideButton.onClick.AddListener(OnGuideButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        yesQuitButton.onClick.AddListener(OnYesQuitButtonClick);
        noQuitButton.onClick.AddListener(OnNoQuitButtonClick);
    }

    private void OnPlayButtonClick() =>  MySceneManager.Instance.SwitchScene(SceneEnum.Game, true);
    
    public void OnGuideButtonClick() => guidePanel.SetActive(!guidePanel.activeSelf);
    public void CloseGuidePanel() => guidePanel.SetActive(false);

    public void OnSettingsButtonClick() => settingsPanel.SetActive(!settingsPanel.activeSelf);
    public void CloseSettingsPanel() => settingsPanel.SetActive(false);

    private void OnQuitButtonClick() => quitConfirmationPanel.SetActive(!quitConfirmationPanel.activeSelf);
    private void OnYesQuitButtonClick() => MySceneManager.Instance.QuitGame();
    private void OnNoQuitButtonClick() => quitConfirmationPanel.SetActive(false);
}
