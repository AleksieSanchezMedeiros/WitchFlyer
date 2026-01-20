using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState state { get; private set; } = GameState.Playing;

    public event Action<GameState, GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        //Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Add anything in here while its still 
        SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        if (state == newState) return;

        GameState oldState = state;
        state = newState;

        switch (state) {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }

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

    public bool IsPlaying => state == GameState.Playing; 
}
