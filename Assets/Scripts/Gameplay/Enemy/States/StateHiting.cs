using Assets.Scripts.Gameplay.Systems;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StateHiting : StateBase
{
    private bool isAttacking = false;
    private readonly string animationName = "Throw";
    public override void Initialize(FsmNPCManager fsmManager, Animator animator, EnemySettingsSO enemySettingsSO, NavMeshAgent agent, Transform player, bool isCivil, HealthSystem healthSystem, CapsuleCollider capsuleCollider, Transform firePoint)
    {
        base.Initialize(fsmManager, animator, enemySettingsSO, agent, player, isCivil, healthSystem, capsuleCollider, firePoint);

        this.stateType = StateType.Hiting;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        isAttacking = false;
    }

    public override void OnUpdate()
    {
        if (healthSystem.GetCurrentLife() <= 0)
        {
            fsmManager.SwitchState(fsmManager.FindState(StateType.Dying));
            return;
        }

        if (!IsPlayerNearby())
        {
            fsmManager.SwitchState(fsmManager.FindState(StateType.Running));
            return;
        }

        Vector3 direction = GetDirectionToPlayer();
        RotateToPlayer(direction);

        if (!isAttacking)
            fsmManager.StartManagedCoroutine(AttackRoutine(direction));
    }



    public override void OnExit()
    {
        isAttacking = false;
    }

    public override void OnAnimatorIK(int layerIndex)
    {
        if (player == null) return;

        animator.SetLookAtWeight(1f, 0.3f, 0.6f, 1f, 0.5f);
        animator.SetLookAtPosition(player.transform.position);
    }

    private Vector3 GetDirectionToPlayer()
    {
        Vector3 direction = (player.position - firePoint.transform.position).normalized;
        return direction;
    }

    private void RotateToPlayer(Vector3 direction)
    {
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        firePoint.rotation = Quaternion.RotateTowards(
            firePoint.rotation,
             targetRotation,
             agent.angularSpeed * Time.deltaTime
         );
    }

    private IEnumerator AttackRoutine(Vector3 direction)
    {
        animator.Play(animationName, 0, 0f);
        isAttacking = true;
        direction += new Vector3(0, 0.1f, 0);
        IPoolable poolable = EnemyBulletPool.Instance.Get();
        MonoBehaviour mb = poolable as MonoBehaviour;
        mb.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        mb.GetComponent<ProjectileController>().Launch(direction, enemySettingsSO.Damage);

        yield return new WaitForSeconds(enemySettingsSO.AttackCooldown);

        isAttacking = false;
    }
}