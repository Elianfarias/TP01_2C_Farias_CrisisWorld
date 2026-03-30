using UnityEngine;

[CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/Enemy")]
public class EnemySettingsSO : ScriptableObject
{
    [SerializeField] private int damage;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private float obstacleDetectionRadius = 2f;
    [SerializeField] private float playerDetectionRadius = 50f;
    [SerializeField] private float wanderRadius = 15f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask playerLayer;

    public int Damage { get { return damage; } }
    public float WaitTime { get { return waitTime; } }
    public float ObstacleDetectionRadius { get { return obstacleDetectionRadius; } }
    public float PlayerDetectionRadius { get { return playerDetectionRadius; } }
    public float WanderRadius { get { return wanderRadius; } }
    public LayerMask ObstacleLayer { get { return obstacleLayer; } }
    public LayerMask PlayerLayer { get { return playerLayer; } }
}
