using UnityEngine;
using UnityEngine.AI;

public abstract class StateBase
{
    protected static readonly int State = Animator.StringToHash("State");
    public StateType stateType;
    
    protected FsmEnemyManager fsmManager;
    protected Animator animator;
    protected EnemySettingsSO enemySettingsSO;
    protected NavMeshAgent agent;
    protected GameObject player;

    public virtual void Initialize(FsmEnemyManager fsmManager, 
        Animator animator, 
        EnemySettingsSO enemySettingsSO, 
        NavMeshAgent agent,
        GameObject player
        )
    {
        this.fsmManager = fsmManager;
        this.animator = animator;
        this.enemySettingsSO = enemySettingsSO;
        this.agent = agent;
        this.player = player;
    }

    public virtual void OnEnter()
    {
        animator.SetInteger(State, (int)stateType);
    }
    public abstract void OnUpdate();
    public abstract void OnExit();
    public abstract void OnAnimatorIK(int layerIndex);

    protected bool IsPlayerNearby()
    {
        return Physics.CheckSphere(fsmManager.transform.position, enemySettingsSO.PlayerDetectionRadius, enemySettingsSO.PlayerLayer);
    }
}
