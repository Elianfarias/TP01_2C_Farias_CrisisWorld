using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text txtCivil;
    [SerializeField] private TMP_Text txtEnemy;
    public int civilMaxCount = 4;
    public int civilCount = 4;
    public int enemyMaxCount = 8;
    public int enemyCount = 8;


    public static ScoreManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        civilCount = civilMaxCount;
        txtCivil.text = civilMaxCount + "/" + civilMaxCount;
        enemyCount = enemyMaxCount;
        txtEnemy.text = enemyMaxCount + "/" + enemyMaxCount;
    }

    public void LessCivilScore()
    {
        civilCount--;
        txtCivil.text = civilCount + "/" + civilMaxCount;

        if (civilCount <= 0)
            GameStateManager.Instance.SetGameState(GameState.GAME_OVER);
    }

    public void LessEnemyScore()
    {
        enemyCount--;
        txtEnemy.text = enemyCount + "/" + enemyMaxCount;

        if (enemyCount <= 0)
            GameStateManager.Instance.SetGameState(GameState.WIN);
    }
}
