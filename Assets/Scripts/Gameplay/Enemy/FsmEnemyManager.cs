using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FsmEnemyManager : MonoBehaviour
{
    [SerializeField] private EnemySettingsSO enemySettingsSO;

    private readonly IList<StateBase> stateBases = new List<StateBase>();
    private StateBase currentState;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent _agent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();

        stateBases.Add(new StateRunning());
        stateBases.Add(new StateHiting());

        foreach (var state in stateBases)
            state.Initialize(this, animator, enemySettingsSO, _agent, player);

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
}
