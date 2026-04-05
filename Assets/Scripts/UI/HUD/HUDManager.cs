using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("PlayerLoseHUD")]
    [SerializeField] private GameObject panelPlayerLose;
    [SerializeField] private Button btnReset;
    [SerializeField] private Button btnBackToMenu;
    [Header("PlayerWinHUD")]
    [SerializeField] private GameObject panelPlayerWin;
    [SerializeField] private TextMeshProUGUI descriptionWin;
    [SerializeField] private Button btnWinReset;
    [SerializeField] private Button btnWinBackToMenu;

    private void Awake()
    {
        Instance = this;

        if (btnReset != null)
        {
            btnBackToMenu.onClick.AddListener(BackToMenu);
            btnReset.onClick.AddListener(ResetGame);
        }
        if (btnWinBackToMenu != null)
        {
            btnWinBackToMenu.onClick.AddListener(BackToMenu);
            btnWinReset.onClick.AddListener(ResetGame);
        }
    }

    private void OnDestroy()
    {
        if (btnReset != null)
        {
            btnBackToMenu.onClick.RemoveAllListeners();
            btnReset.onClick.RemoveAllListeners();
        }
        if (btnWinBackToMenu != null)
        {
            btnWinBackToMenu.onClick.RemoveAllListeners();
            btnWinReset.onClick.RemoveAllListeners();
        }
    }

    public void ShowPanelPlayerLose()
    {
        panelPlayerLose.SetActive(true);
    }

    public void ShowPanelPlayerWin()
    {
        descriptionWin.text = "You have rescued " + ScoreManager.Instance.civilCount + " civilians";
        panelPlayerWin.SetActive(true);
    }

    private void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetGame()
    {
        Time.timeScale = 1;
        GameStateManager.Instance.SetGameState(GameState.PLAYING);
        SceneManager.LoadScene("Level1");
    }
}
