using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameOver;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameOver;
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
