using Assets.Scripts.Gameplay.Systems;
using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneShooter : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO data;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Rope rope;

    private int groundLayer;
    private float nextTimeShoot;
    private float nextTimeThrowRope;

    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        if (nextTimeThrowRope > Time.time)
            rope.startPoint = transform.position;
        else
        {
            if (rope.IsActiveRope())
                rope.SetActiveRope(false);
        }
    }

    private void OnAttack(InputValue value)
    {
        if (value.isPressed && nextTimeShoot < Time.time)
        {
            nextTimeShoot = Time.time + data.CdAttack;
            Shoot();
        }
    }

    private void OnThrowRope(InputValue value)
    {
        if (value.isPressed 
            && nextTimeThrowRope < Time.time
            && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
        {
            rope.SetActiveRope(true);
            nextTimeThrowRope = Time.time + data.CdRope;
            rope.endPoint = hitInfo.point;
        }
    }

    private void Shoot()
    {
        IPoolable poolable = PlayerBulletPool.Instance.Get();
        MonoBehaviour mb = poolable as MonoBehaviour;
        mb.transform.SetPositionAndRotation(firePoint.position, gameObject.transform.rotation);
        mb.GetComponent<ProjectileController>().Launch(firePoint.forward, data.Damage);
    }
}
