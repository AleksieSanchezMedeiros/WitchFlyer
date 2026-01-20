using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance { get; private set; }

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float loadingScreenDuration = 2f;
    private bool isPaused;

    [Header("Scene Input")]
    [SerializeField] private KeyCode pauseKey = KeyCode.O;
    [SerializeField] private KeyCode restartKey = KeyCode.P;

    public SceneEnum currentScene = SceneEnum.MainMenu;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (loadingScreen != null) DontDestroyOnLoad(loadingScreen);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void Update()
    {
        HandleSceneInput();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
            GameManager.Instance.SetState(GameState.Playing);
    }

    /// <summary>
    /// Use this class to switch scenes by specifying the scene name. Use the second boolean parameter to toggle the loading screen.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="withLoadingScreen"></param>
    public void SwitchScene(SceneEnum scene, bool withLoadingScreen = false)
    {
        currentScene = scene;
        if (withLoadingScreen) {
            StartCoroutine(LoadSceneWithLoadingScreen(scene));
        } else {
            SceneManager.LoadScene(scene.ToString());
        }
    }

    private IEnumerator LoadSceneWithLoadingScreen(SceneEnum scene)
    {
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        yield return new WaitForSecondsRealtime(loadingScreenDuration);

        while (!asyncLoad.isDone) {
            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGameToggle()
    {
        if (SceneManager.GetActiveScene().name != "GameScene") return;
        GameManager.Instance.TogglePause();
    }

    private void HandleSceneInput()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.pKey.wasPressedThisFrame) RestartScene();
        // if (kb.pKey.wasPressedThisFrame || kb.escapeKey.wasPressedThisFrame) PauseGameToggle();
    }

    public bool LoadingScreenIsNotActive()
    {
        return !loadingScreen.activeSelf;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

public enum SceneEnum
{
    MainMenu,
    GameScene
}