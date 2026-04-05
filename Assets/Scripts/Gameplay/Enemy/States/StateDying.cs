using UnityEngine;
using UnityEngine.AI;

public class StateDying : StateBase
{
    public override void Initialize(FsmNPCManager fsmManager, Animator animator, EnemySettingsSO enemySettingsSO, NavMeshAgent agent, GameObject player, bool isCivil, HealthSystem healthSystem, CapsuleCollider capsuleCollider, Transform firePoint)
    {
        base.Initialize(fsmManager, animator, enemySettingsSO, agent, player, isCivil, healthSystem, capsuleCollider, firePoint);

        this.stateType = StateType.Dying;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        capsuleCollider.enabled = false;
        agent.isStopped = true;
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnAnimatorIK(int layerIndex)
    {
    }
}