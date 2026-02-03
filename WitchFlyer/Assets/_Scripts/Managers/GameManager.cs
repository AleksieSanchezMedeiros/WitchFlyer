using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState state { get; private set; } = GameState.Playing;

    public static Action<GameState, GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        //Instance = this;
        //DontDestroyOnLoad(gameObject);

        SetState(GameState.None);
    }

    private void Start()
    {
        // Add anything in here while its still 
        //SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        if (state == newState) return;

        GameState oldState = state;
        state = newState;
        Debug.Log("STATE UPDATE!");
        switch (state) {
            case GameState.Playing:
                Debug.Log("PLAYING STATE");
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Debug.Log("PAUSED STATE");
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Debug.Log("GAME OVER!");
                Time.timeScale = 0f;
                break;
            case GameState.GameComplete:
                Debug.Log("GAME COMPLETE");
                Time.timeScale = 0f;
                break;
            default:
                break;
        }
        Debug.Log("WHERE");
        OnStateChanged?.Invoke(oldState, newState);
    }

    public void TogglePause()
    {
        if (state == GameState.GameOver) return;
        SetState(state == GameState.Paused ? GameState.Playing : GameState.Paused);
    }

    public void GameOver()
    {
        SetState(GameState.GameOver);
    }

    public void GameComplete()
    {
        SetState(GameState.GameComplete);
    }

    public void Restart()
    {
        MySceneManager.Instance.RestartScene();
    }

    public void ReturnToMenu()
    {
        MySceneManager.Instance.SwitchScene(SceneEnum.MainMenu);
    }

    public bool IsPlaying => state == GameState.Playing; 
}

public enum GameState
{
    None,
    Playing,
    Paused,
    GameOver,
    GameComplete
}
