using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text txtCivil;
    [SerializeField] private TMP_Text txtEnemy;
    public int civilMaxCount = 4;
    public int civilCount = 0;
    public int enemyMaxCount = 8;
    public int enemyCount = 0;


    public static ScoreManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        txtCivil.text = civilCount + "/" + civilMaxCount;
        txtEnemy.text = enemyCount + "/" + enemyMaxCount;
    }

    public void LessEnemyScore()
    {
        enemyCount++;
        txtEnemy.text = enemyCount + "/" + enemyMaxCount;
    }

    public void LessCivilScore()
    {
        enemyCount++;
        txtCivil.text = civilCount + "/" + civilMaxCount;
    }
}
