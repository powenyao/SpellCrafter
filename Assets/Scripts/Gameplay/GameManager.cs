using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject levelTemplates;

    [SerializeField]
    LauncherController launcherController;

    [SerializeField]
    LevelUI levelUI;

    [SerializeField]
    float switchDelay = 2f;
    [SerializeField]
    bool autoProgress = true;

    private int numLevels;
    private int currentLevelIndex = -1;
    private GameObject currentLevel;
    private LevelDetails currentLevelData;
    private IEnumerable<ShootingTarget> currentTargets;
    private int _numSpellsCast = 0;
    private int _numEnemiesAlive = 0;
    private float _totalSpellCost = 0;

    public int numSpellsCast
    {
        get { return _numSpellsCast; }
    }

    public float totalSpellCost
        {
        get { return _totalSpellCost; }
    }

    public int numEnemiesAlive
    {
        get { return _numEnemiesAlive; }
        set
        {
            _numEnemiesAlive = value;
            levelUI.SetEnemyCount(value);
        }
    }

    [Header("Debug")]

    [SerializeField]
    InputActionReference goNextAction;
    [SerializeField]
    InputActionReference goPrevAction;
    [SerializeField]
    InputActionReference restartAction;

    private void SetSpellStats(int count, float cost)
    {
        _numSpellsCast = count;
        _totalSpellCost = cost;
        levelUI.SetSpellStats(count, cost);
    }

    void Awake()
    {
        launcherController.OnSpellLaunched += (cost) =>
        {
            SetSpellStats(numSpellsCast + 1, totalSpellCost + cost);
        };

        goNextAction.action.performed += obj => StartCoroutine(NextLevelAsync());
        goPrevAction.action.performed += obj => StartCoroutine(PreviousLevelAsync());
        restartAction.action.performed += obj => StartCoroutine(RestartLevelAsync());
    }

    // Start is called before the first frame update
    void Start()
    {
        numLevels = levelTemplates.transform.childCount;
        StartCoroutine(NextLevelAsync());
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void LoadCurrentLevel()
    {
        if (currentLevelIndex == -1)
            return;

        GameObject template = levelTemplates.transform.GetChild(currentLevelIndex).gameObject;
        currentLevel = Instantiate(template, transform, true);
        currentLevel.name = "CurrentLevel";
        currentLevelData = currentLevel.GetComponent<LevelDetails>();

        int enemyCount = 0;
        currentTargets = currentLevel.GetComponentsInChildren<ShootingTarget>();
        foreach (ShootingTarget enemy in currentTargets)
        {
            enemy.OnTargetDestroyed += HandleTargetDestroyed;
            enemyCount++;
        }
        numEnemiesAlive = enemyCount;
        SetSpellStats(0, 0);

        currentLevel.SetActive(true);
        levelUI.SetNarration(currentLevelData.TutorialText);
    }

    private void HandleTargetDestroyed(ShootingTarget target)
    {
        numEnemiesAlive--;
        if (numEnemiesAlive == 0)
        {
            levelUI.SetNarration(currentLevelData.CompletionText);
            if (autoProgress)
            {
                StartCoroutine(NextLevelAsync());
            }
        }
    }

    private void UnloadCurrentLevel()
    {
        if (currentLevelIndex == -1)
            return;

        foreach (ShootingTarget enemy in currentTargets)
        {
            enemy.OnTargetDestroyed -= HandleTargetDestroyed;
        }
        Destroy(currentLevel);
    }

    public void JumpToLevel(int levelIndex)
    {
        UnloadCurrentLevel();
        currentLevelIndex = levelIndex;
        LoadCurrentLevel();
    }

    [ContextMenu("Next Level")]
    public IEnumerator NextLevelAsync()
    {
        bool shouldEnd = currentLevelData == null ? false : currentLevelData.IsFinal;
        UnloadCurrentLevel();
        yield return new WaitForSeconds(switchDelay);
        if (shouldEnd)
        {
            SceneManager.LoadScene("EndScreen");
        }
        else
        {
            currentLevelIndex = (currentLevelIndex + 1) % numLevels;
            LoadCurrentLevel();
        }
    }

    [ContextMenu("Previous Level")]
    public IEnumerator PreviousLevelAsync()
    {
        UnloadCurrentLevel();
        yield return new WaitForSeconds(switchDelay);
        if (currentLevelIndex == -1)
            currentLevelIndex = numLevels - 1;
        else
            currentLevelIndex = (numLevels + currentLevelIndex - 1) % numLevels;
        LoadCurrentLevel();
    }

    [ContextMenu("Restart Level")]
    public IEnumerator RestartLevelAsync()
    {
        UnloadCurrentLevel();
        yield return new WaitForSeconds(switchDelay);
        LoadCurrentLevel();
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelAsync());
    }

    [ContextMenu("End Level")]
    public void EndLevel()
    {
        UnloadCurrentLevel();
        currentLevelIndex = -1;
    }
}
