using UnityEngine;
using UnityEngine.AI;

public class StateRunning : StateBase
{
    private float _waitTimer;
    public override void Initialize(FsmEnemyManager fsmManager, Animator animator, EnemySettingsSO enemySettingsSO, NavMeshAgent agent, GameObject player)
    {
        base.Initialize(fsmManager, animator, enemySettingsSO, agent, player);

        this.stateType = StateType.Running;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        agent.isStopped = false;
    }

    public override void OnUpdate()
    {
        if (!agent.isOnNavMesh || !agent.isActiveAndEnabled) return;

        if (IsObstacleNearby() || agent.remainingDistance <= agent.stoppingDistance)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= enemySettingsSO.WaitTime)
            {
                MoveToRandomPosition();
                _waitTimer = 0f;
            }
        }

        if (IsPlayerNearby())
        {
            StateBase hitingState = fsmManager.FindState(StateType.Hiting);
            fsmManager.SwitchState(hitingState);
        }
    }

    public override void OnExit()
    {
        agent.isStopped = true;
    }

    public override void OnAnimatorIK(int layerIndex)
    {
    }

    private bool IsObstacleNearby()
    {
        return Physics.CheckSphere(fsmManager.transform.position, enemySettingsSO.ObstacleDetectionRadius, enemySettingsSO.ObstacleLayer);
    }

    private void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * enemySettingsSO.WanderRadius;
        randomDirection += fsmManager.transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, enemySettingsSO.WanderRadius, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }
}
