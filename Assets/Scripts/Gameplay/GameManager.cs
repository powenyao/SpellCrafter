using Assets.Scripts.Gameplay;
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
    private int _numSpellsCastThisLevel = 0;
    private int _numSpellsCastThisPlaythrough = 0;
    private int _numEnemiesAlive = 0;
    private float _totalSpellCostThisLevel = 0;
    private float _totalSpellCostThisPlaythrough = 0;

    public int numSpellsCast
    {
        get { return _numSpellsCastThisLevel; }
    }

    public float totalSpellCost
    {
        get { return _totalSpellCostThisLevel; }
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

    private void SetLevelScoreStats(int count, float cost)
    {
        _numSpellsCastThisLevel = count;
        _totalSpellCostThisLevel = cost;
        levelUI.SetSpellStats(count, cost);
    }

    private void IncrementPlaythroughScoreStats(int count, float cost)
    {
        _numSpellsCastThisPlaythrough += count;
        _totalSpellCostThisPlaythrough += cost;
    }

    void Awake()
    {
        launcherController.OnSpellLaunched += (cost) =>
        {
            SetLevelScoreStats(numSpellsCast + 1, totalSpellCost + cost);
        };

        if (DebugOption.IsAvailable)
        {
            goNextAction.action.performed += obj => StartCoroutine(NextLevelAsync());
            goPrevAction.action.performed += obj => StartCoroutine(PreviousLevelAsync());
            restartAction.action.performed += obj => StartCoroutine(RestartLevelAsync());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerScores.InitializeBestScore();
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
        SetLevelScoreStats(0, 0);

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
        // TODO: Ask Vinay how we want to handle total score in this scenario. Probably need to flag that the total score is invalid and not save it if levels are skipped.
        UnloadCurrentLevel();
        currentLevelIndex = levelIndex;
        LoadCurrentLevel();
    }

    [ContextMenu("Next Level")]
    public IEnumerator NextLevelAsync()
    {
        IncrementPlaythroughScoreStats(_numSpellsCastThisLevel, _totalSpellCostThisLevel);
        bool shouldEnd = currentLevelData == null ? false : currentLevelData.IsFinal;
        UnloadCurrentLevel();
        yield return new WaitForSeconds(switchDelay);
        if (shouldEnd)
        {
            PlayerScores.SetNewScore(_numSpellsCastThisPlaythrough, _totalSpellCostThisPlaythrough);
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