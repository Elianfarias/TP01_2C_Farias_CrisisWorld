using Assets.Scripts.Gameplay.Systems;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneShooter : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO data;
    [SerializeField] private Transform firePoint;

    private float nextTimeShoot;

    private void OnAttack(InputValue value)
    {
        if(value.isPressed && nextTimeShoot < Time.time)
        {
            nextTimeShoot = Time.time + data.CdAttack;
            Shoot();
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
