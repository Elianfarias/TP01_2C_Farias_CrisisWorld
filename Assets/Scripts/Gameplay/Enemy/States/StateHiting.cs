using UnityEngine;
using UnityEngine.AI;

public class StateHiting : StateBase
{
    public override void Initialize(FsmEnemyManager fsmManager, Animator animator, EnemySettingsSO enemySettingsSO, NavMeshAgent agent, GameObject player)
    {
        base.Initialize(fsmManager, animator, enemySettingsSO, agent, player);

        this.stateType = StateType.Hiting;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        Vector3 direction = (player.transform.position - fsmManager.transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 45f, 0f);

        fsmManager.transform.rotation = Quaternion.RotateTowards(
            fsmManager.transform.rotation,
            targetRotation,
            agent.angularSpeed * Time.deltaTime
            );

        if (!IsPlayerNearby())
        {
            StateBase runningState = fsmManager.FindState(StateType.Running);
            fsmManager.SwitchState(runningState);
        }
    }

    public override void OnExit()
    {
    }

    public override void OnAnimatorIK(int layerIndex)
    {
        if (player == null) return;

        animator.SetLookAtWeight(1f, 0.3f, 0.6f, 1f, 0.5f);
        animator.SetLookAtPosition(player.transform.position);
    }
}