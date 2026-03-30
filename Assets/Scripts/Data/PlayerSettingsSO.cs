using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Player")]
public class PlayerSettingsSO : ScriptableObject
{
    [SerializeField] private int damage = 10;
    [Header("Movimiento")]
    [SerializeField] private float force = 10f;
    [SerializeField] private float verticalForce = 8f;
    [Header("Rotacion")]
    [SerializeField] private float rotationSpeedX = 80f;
    [SerializeField] private float rotationSpeedY = 60f;
    [SerializeField] private float maxPitchAngle = 60f;
    [SerializeField] private float maxHorizontalSpeed = 10f;
    [SerializeField] private float maxVerticalSpeed = 5f;
    [Header("Capas colisionable")]
    [SerializeField] private LayerMask layerCollision;


    public int Damage { get { return damage; } }
    public float Force { get { return force; } }
    public float VerticalForce { get { return verticalForce; } }
    public float RotationSpeedX { get { return rotationSpeedX; } }
    public float RotationSpeedY { get { return rotationSpeedY; } }
    public float MaxPitchAngle { get { return maxPitchAngle; } }
    public float MaxHorizontalSpeed { get { return maxHorizontalSpeed; } }
    public float MaxVerticalSpeed { get { return maxVerticalSpeed; } }
    public LayerMask LayerCollision { get { return layerCollision; } }
}
