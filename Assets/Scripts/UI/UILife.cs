using UnityEngine;
using UnityEngine.UI;

public class UILife : MonoBehaviour
{
    [SerializeField] private HealthSystem target;
    [SerializeField] private Image barLife;


    private void Awake()
    {
        target.OnLifeUpdated += HealthSystem_onLifeUpdated;
        target.OnHealing += HealthSystem_onLifeUpdated;
        target.OnDie += HealthSystem_onDie;
    }

    private void OnDestroy()
    {
        target.OnLifeUpdated -= HealthSystem_onLifeUpdated;
        target.OnHealing -= HealthSystem_onLifeUpdated;
        target.OnDie -= HealthSystem_onDie;
    }

    public void HealthSystem_onLifeUpdated(float current, float max)
    {
        float lerp = current / max;
        barLife.fillAmount = lerp;
    }


    private void HealthSystem_onDie()
    {
        barLife.fillAmount = 0;
        GameStateManager.Instance.SetGameState(GameState.GAME_OVER);
    }
}