using Assets.Scripts.Gameplay.Systems;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IPoolable
{
    [Header("Config")]
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lifetime = 5f;

    [Header("VFX")]
    [SerializeField] private GameObject rocketExplosion;
    [SerializeField] private ParticleSystem disableOnHit;

    [Header("References")]
    [SerializeField] private MeshRenderer projectileMesh;
    [SerializeField] private AudioSource inFlightAudioSource;

    private float damage;
    private float elapsedTime;
    private bool targetHit;
    private Vector3 origin;
    private Vector3 initialVelocity;
    private Coroutine returnCoroutine;

    public void OnGetFromPool()
    {
        targetHit = false;
        elapsedTime = 0f;

        if (projectileMesh != null) projectileMesh.enabled = true;
        if (inFlightAudioSource != null) inFlightAudioSource.Play();
        if (disableOnHit != null) disableOnHit.Play();

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = true;
    }

    public void OnReturnToPool()
    {
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
    }

    private void Update()
    {
        if (targetHit) return;

        elapsedTime += Time.deltaTime;

        transform.position = CalculatePosition(elapsedTime);
        transform.rotation = CalculateRotation(elapsedTime);

        if (elapsedTime >= lifetime)
            ReturnToPool();
    }

    private Vector3 CalculatePosition(float t)
    {
        return origin
            + initialVelocity * t
            + (t * t) * 0.5f * Physics.gravity;
    }

    private Quaternion CalculateRotation(float t)
    {
        Vector3 velocity = initialVelocity + Physics.gravity * t;
        return velocity != Vector3.zero
            ? Quaternion.LookRotation(velocity)
            : transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit) return;

        if (collision.collider.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
            healthSystem.DoDamage(damage);

        targetHit = true;
        Explode();
        returnCoroutine = StartCoroutine(ReturnAfterDelay());
    }

    private void Explode()
    {
        Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation);

        if(projectileMesh != null)
            projectileMesh.enabled = false;
        
        inFlightAudioSource.Stop();
        disableOnHit.Stop();

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = false;
    }

    private System.Collections.IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        BulletPool.Instance.Return(this);
    }

    public void Launch(Vector3 direction, float damage)
    {
        this.damage = damage;
        origin = transform.position;
        initialVelocity = direction * speed;
    }
}