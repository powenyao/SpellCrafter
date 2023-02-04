using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Enum_WaveEnemies
{
    StationaryShootingTarget,
    MovingShootingTarget,
    Skeleton,
    SkeletonSoldier,
    SkeletonKnight,
    Goblin,
    GoblinWarrior,
    RockGolem,
}

[Serializable]
public class EnemyWave
{
    public string name;
    public int count = 3;
    public float spawnCooldown = 2f;
    public List<Enum_WaveEnemies> waveEnemies;
}

public enum Enum_WaveState
{
    Waiting,
    Spawning
    
}

public class Subservice_WaveGenerator : XrosSubservice
{
    [SerializeField]
    private Dictionary<Enum_WaveEnemies, GameObject> _enemeyList = new Dictionary<Enum_WaveEnemies, GameObject>();

    private List<EnemyWave> _enemyWaveList = new List<EnemyWave>();

    [SerializeField]
    private List<Transform> spawnTransformGroundList = new List<Transform>();

    [SerializeField]
    private List<Transform> spawnTransformAerialList = new List<Transform>();

    [SerializeField]
    private GameObject PF_StationaryShootingTarget;

    [SerializeField]
    private GameObject PF_MovingShootingTarget;

    [SerializeField]
    private GameObject PF_Skeleton;

    [SerializeField]
    private GameObject PF_SkeletonSoldier;

    [SerializeField]
    private GameObject PF_SkeletonKnight;

    [SerializeField]
    private GameObject PF_Goblin;

    [SerializeField]
    private GameObject PF_GoblinWarrior;

    [SerializeField]
    private GameObject PF_RockGolem;

    private List<GameObject> _currentWaveEnemiesList = new List<GameObject>();

    private int _currentWaveId = 0;
    private EnemyWave _currentWave;
    private Enum_WaveState _waveState = Enum_WaveState.Waiting;
    private float _timeUntilNextSpawn = 0f;
    private int _currentWaveSpawnsNumber = 0;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        Core.Ins.Subservices.RegisterService(nameof(Subservice_WaveGenerator), this);
        Core.Ins.Debug.AddDebugCodeForKeyboard(this.gameObject, nameof(SpellcastingTester), nameof(StartWave),
            KeyCode.G, () => { StartWave(); });
        Core.Ins.Debug.AddDebugCodeForKeyboard(this.gameObject, nameof(SpellcastingTester), nameof(GenerateWaveEnemy),
            KeyCode.H, () => { GenerateWaveEnemy(); });
        Core.Ins.Debug.AddDebugCodeForKeyboard(this.gameObject, nameof(SpellcastingTester), nameof(RestartWave),
            KeyCode.J, () => { GenerateWaveEnemy(); });

        var waveEnemiesList = new List<Enum_WaveEnemies>();
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        _enemyWaveList.Add(new EnemyWave { name = "Wave 1", count = 3, spawnCooldown = 2f, waveEnemies = waveEnemiesList });
        waveEnemiesList = new List<Enum_WaveEnemies>();
        waveEnemiesList.Add(Enum_WaveEnemies.MovingShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.MovingShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.MovingShootingTarget);
        _enemyWaveList.Add(new EnemyWave { name = "Wave 2", count = 3, spawnCooldown = 2f, waveEnemies = waveEnemiesList });
        waveEnemiesList = new List<Enum_WaveEnemies>();
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        _enemyWaveList.Add(new EnemyWave { name = "Wave 3", count = 3, spawnCooldown = 2f, waveEnemies = waveEnemiesList });
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        waveEnemiesList.Add(Enum_WaveEnemies.StationaryShootingTarget);
        _enemyWaveList.Add(new EnemyWave { name = "Wave 4", count = 3, spawnCooldown = 2f, waveEnemies = waveEnemiesList });
    }

    public void RegisterSpawnTransform(SpawnTransformProperty property)
    {
        switch (property.spawnType)
        {
            case Enum_SpawnType.Any:
                break;
            case Enum_SpawnType.Ground:
                spawnTransformGroundList.Add(property.transform);
                break;
            case Enum_SpawnType.Aerial:
                spawnTransformAerialList.Add(property.transform);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(property), property, null);
        }
    }

    void OnDisable()
    {
        Core.Ins.Subservices.UnregisterService(nameof(Subservice_WaveGenerator), this);
    }

    void Awake()
    {
        _enemeyList.Add(Enum_WaveEnemies.StationaryShootingTarget, PF_StationaryShootingTarget);
        _enemeyList.Add(Enum_WaveEnemies.MovingShootingTarget, PF_MovingShootingTarget);
        _enemeyList.Add(Enum_WaveEnemies.Skeleton, PF_Skeleton);
        _enemeyList.Add(Enum_WaveEnemies.SkeletonSoldier, PF_SkeletonSoldier);
        _enemeyList.Add(Enum_WaveEnemies.SkeletonKnight, PF_SkeletonKnight);
        _enemeyList.Add(Enum_WaveEnemies.Goblin, PF_Goblin);
        _enemeyList.Add(Enum_WaveEnemies.GoblinWarrior, PF_GoblinWarrior);
        _enemeyList.Add(Enum_WaveEnemies.RockGolem, PF_RockGolem);
    }

    void Update()
    {
        if (_waveState == Enum_WaveState.Spawning)
        {
            _timeUntilNextSpawn -= Time.deltaTime;
            if (_timeUntilNextSpawn <= 0)
            {
                GenerateWaveEnemy();
            }
        }
    }
    public GameObject GetEnemy(Enum_WaveEnemies enemy)
    {
        var pf = _enemeyList[enemy];
        var go = Instantiate(pf);
        return go;
    }

    public void StartWave()
    {
        if (_currentWaveId < _enemyWaveList.Count)
        {
            _currentWave = _enemyWaveList[_currentWaveId];

            _waveState = Enum_WaveState.Spawning;
            GenerateWaveEnemy();
            _currentWaveId++;
            
        }
        else
        {
            Dev.Log("Max Wave Reached " + _currentWaveId + "/"+ _enemyWaveList.Count);
        }
    }

    public void RestartWave()
    {
        foreach (var VARIABLE in _currentWaveEnemiesList)
        {
            Destroy(VARIABLE.gameObject);
        }
        
        _currentWaveEnemiesList.Clear();
        _currentWaveId = 0;
    }
    public void GenerateWaveEnemy()
    {
        var go = GetEnemy(_currentWave.waveEnemies[_currentWaveSpawnsNumber]);
        _currentWaveEnemiesList.Add(go);
        _currentWaveSpawnsNumber++;

        //Handle Spawn Location
        if (go.TryGetComponent(out WaveEnemyProperty property))
        {
            //var property = go.GetComponent<WaveEnemyProperty>();
            go.transform.position = GetSpawnPosition(property);
            go.transform.localRotation = this.transform.rotation;
        }
        else
        {
            Dev.LogWarning("Cannot find WaveEnemyProperty script on GO " + go.name);
        }

        _timeUntilNextSpawn = _currentWave.spawnCooldown;
        
        if (_currentWaveSpawnsNumber >= _currentWave.count )
        {
            _currentWaveSpawnsNumber = 0;
            _waveState = Enum_WaveState.Waiting;
        }
    }

    private Vector3 GetSpawnPosition(WaveEnemyProperty property)
    {
        Vector3 position = Vector3.zero;
        switch (property.spawnType)
        {
            case Enum_SpawnType.Any:

                break;
            case Enum_SpawnType.Ground:
                position = spawnTransformGroundList[Random.Range(0, spawnTransformGroundList.Count)].position;
                break;
            case Enum_SpawnType.Aerial:
                position = spawnTransformAerialList[Random.Range(0, spawnTransformAerialList.Count)].position;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        position += property.spawnOffset;
        return position;
    }
}