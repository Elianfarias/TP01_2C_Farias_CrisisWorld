using UnityEngine;
using UnityEngine.AI;

public class StateRunning : StateBase
{
    private float nextWanderTime;

    public override void Initialize(FsmNPCManager fsmManager, Animator animator, EnemySettingsSO enemySettingsSO, NavMeshAgent agent, GameObject player, bool isCivil, HealthSystem healthSystem, CapsuleCollider capsuleCollider, Transform firePoint)
    {
        base.Initialize(fsmManager, animator, enemySettingsSO, agent, player, isCivil, healthSystem, capsuleCollider, firePoint);
        this.stateType = StateType.Running;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = false;
        nextWanderTime = 0f;
    }

    public override void OnUpdate()
    {
        if (!agent.isOnNavMesh || !agent.isActiveAndEnabled) return;

        if (healthSystem.GetCurrentLife() <= 0)
        {
            StateBase dyingState = fsmManager.FindState(StateType.Dying);
            fsmManager.SwitchState(dyingState);
            return;
        }

        if (IsPlayerNearby())
        {
            StateBase hitingState = fsmManager.FindState(StateType.Hiting);
            fsmManager.SwitchState(hitingState);
            return;
        }

        bool arrivedAtDestination = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;

        if ((IsObstacleNearby() || arrivedAtDestination || (isCivil && IsEnemyNearby())) && Time.time >= nextWanderTime)
        {
            MoveToRandomPosition();
            nextWanderTime = Time.time + enemySettingsSO.WanderCooldown;
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
        if (NavMesh.FindClosestEdge(fsmManager.transform.position, out NavMeshHit hit, NavMesh.AllAreas))
            return hit.distance < enemySettingsSO.ObstacleDetectionRadius;

        return false;
    }

    private bool IsEnemyNearby()
    {
        return Physics.CheckSphere(fsmManager.transform.position, enemySettingsSO.ObstacleDetectionRadius, enemySettingsSO.EnemyLayer);
    }

    private void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * enemySettingsSO.WanderRadius;
        randomDirection += fsmManager.transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, enemySettingsSO.WanderRadius, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }
}