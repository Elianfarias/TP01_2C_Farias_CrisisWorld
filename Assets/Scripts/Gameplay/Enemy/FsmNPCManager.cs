using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FsmNPCManager : MonoBehaviour
{
    [SerializeField] private EnemySettingsSO enemySettingsSO;
    [SerializeField] private bool isCivil;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Animator animator;
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] Transform firePoint;

    private readonly IList<StateBase> stateBases = new List<StateBase>();
    private StateBase currentState;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");


        stateBases.Add(new StateRunning());
        stateBases.Add(new StateHiting());
        stateBases.Add(new StateDying());

        foreach (var state in stateBases)
            state.Initialize(this, animator, enemySettingsSO, _agent, player, isCivil, healthSystem, capsuleCollider, firePoint);

        currentState = FindState(StateType.Running);
    }

    private void Update()
    {
        currentState.OnUpdate();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        currentState.OnAnimatorIK(layerIndex);
    }

    public void SwitchState(StateBase state)
    {
        if (currentState == state) return;

        currentState.OnExit();
        currentState = state;
        currentState.OnEnter();
    }

    public StateBase FindState(StateType stateType)
    {
        foreach (var state in stateBases)
        {
            if (state.stateType == stateType)
                return state;
        }

        return null;
    }

    public Coroutine StartManagedCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
}
