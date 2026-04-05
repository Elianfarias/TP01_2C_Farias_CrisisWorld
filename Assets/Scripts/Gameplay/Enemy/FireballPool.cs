using Assets.Scripts.Gameplay.GameSystem.Object_Pool;
using Assets.Scripts.Gameplay.Systems;
using UnityEngine;
using UnityEngine.Pool;

public class FireballPool : PoolBase
{
    public static FireballPool Instance { get; private set; }

    [SerializeField] private GameObject bulletPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        base.Initialize();
    }

    protected override IPoolable CreateNew()
    {
        GameObject obj = Instantiate(bulletPrefab, transform);
        obj.SetActive(false);
        return obj.GetComponent<IPoolable>();
    }
}