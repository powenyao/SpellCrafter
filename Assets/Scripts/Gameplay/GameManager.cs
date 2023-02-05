using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject levelTemplates;

    [SerializeField]
    LauncherController launcherController;

    [SerializeField]
    LevelUI levelUI;

    private int numLevels;
    private int currentLevelIndex = -1;
    private GameObject currentLevel;
    private IEnumerable<ShootingTarget> currentTargets;
    private int _numSpellsCast = 0;
    private int _numEnemiesAlive = 0;

    public int numSpellsCast
    {
        get { return _numSpellsCast; }
        set
        {
            _numSpellsCast = value;
            levelUI.SetSpellCount(value);
        }
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

    void Awake()
    {
        launcherController.OnSpellLaunched += () =>
        {
            numSpellsCast++;
        };

        goNextAction.action.performed += obj => NextLevel();
        goPrevAction.action.performed += obj => PreviousLevel();
        restartAction.action.performed += obj => RestartLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        numLevels = levelTemplates.transform.childCount;
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

        int enemyCount = 0;
        currentTargets = currentLevel.GetComponentsInChildren<ShootingTarget>();
        foreach (ShootingTarget enemy in currentTargets)
        {
            enemy.OnTargetDestroyed += HandleTargetDestroyed;
            enemyCount++;
        }
        numEnemiesAlive = enemyCount;
        numSpellsCast = 0;

        currentLevel.SetActive(true);
    }

    private void HandleTargetDestroyed(ShootingTarget target)
    {
        numEnemiesAlive--;
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

    void JumpToLevel(int levelIndex)
    {
        UnloadCurrentLevel();
        currentLevelIndex = levelIndex;
        LoadCurrentLevel();
    }

    [ContextMenu("Next Level")]
    void NextLevel()
    {
        UnloadCurrentLevel();
        currentLevelIndex = (currentLevelIndex + 1) % numLevels;
        LoadCurrentLevel();
    }

    [ContextMenu("Previous Level")]
    void PreviousLevel()
    {
        UnloadCurrentLevel();
        if (currentLevelIndex == -1)
            currentLevelIndex = numLevels - 1;
        else
            currentLevelIndex = (numLevels + currentLevelIndex - 1) % numLevels;
        LoadCurrentLevel();
    }

    [ContextMenu("Restart Level")]
    void RestartLevel()
    {
        UnloadCurrentLevel();
        LoadCurrentLevel();
    }

    [ContextMenu("End Level")]
    void EndLevel()
    {
        UnloadCurrentLevel();
        currentLevelIndex = -1;
    }
}
