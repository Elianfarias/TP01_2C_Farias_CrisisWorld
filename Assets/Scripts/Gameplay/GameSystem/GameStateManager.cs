using UnityEngine;

public enum GameState
{
    MAIN_MENU,
    PLAYING,
    PAUSED,
    GAME_OVER,
    TUTORIAL,
    WIN
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private AudioClip clipGameOver;
    public static GameStateManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; } = GameState.PLAYING;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        SetGameState(CurrentGameState);
    }

    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;
        switch (newState)
        {
            case GameState.TUTORIAL:
                AudioController.Instance.PlayBackgroundMusic();
                break;
            case GameState.PLAYING:
                AudioController.Instance.PlayBackgroundMusic();
                break;
            case GameState.PAUSED:
                break;
            case GameState.GAME_OVER:
                Cursor.lockState = CursorLockMode.None;
                AudioController.Instance.StopBackgroundMusic();
                HUDManager.Instance.ShowPanelPlayerLose();
                Time.timeScale = 0;
                break;
            case GameState.WIN:
                Cursor.lockState = CursorLockMode.None;
                AudioController.Instance.StopBackgroundMusic();
                HUDManager.Instance.ShowPanelPlayerWin();
                Time.timeScale = 0;
                break;
        }
    }
}
