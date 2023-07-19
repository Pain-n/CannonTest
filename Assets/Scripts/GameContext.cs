using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public Transform Canvas;

    public CannonController Cannon;
    [HideInInspector]
    public int Vallet;

    private Spawner spawner;
    [HideInInspector]
    public List<EnemyController> EnemiesOnField;
    [HideInInspector]
    public EnemyLibrarySO EnemyLib;
    [HideInInspector]
    public int DifficultyID; //0 for easy, 1 for normal, etc.

    [HideInInspector]
    public int Score;

    private float SpawnDelay;
    [SerializeField]
    private float UpperLimitSpawnDelay;
    [SerializeField]
    private float LowerLimitSpawnDelay;

    public List<IEnumerator> CoroutinesList;

    public static Action UpdateVallet;
    public static Action UpdateEnemiesCount;
    public static Action UpdateDifficulty;
    public static Action UpdateScore;
    private void Awake()
    {
        spawner = new GameObject().AddComponent<Spawner>();
        spawner.name = "Spawner";
        spawner.Init(this);

        CoroutinesList = new List<IEnumerator>();
    }

    private void Start()
    {
        GamePanel gamePanel = Instantiate(Resources.Load<GamePanel>("Prefabs/Panels/GamePanel"), Canvas);
        gamePanel.Init(this);

        Cannon.Init(this);

        EnemyLib = Resources.Load<EnemyLibrarySO>("SO/EnemyLibrary");
        DifficultyID = 0;

        EnemiesOnField = new List<EnemyController>();
        BeginCorounine(SpawnEnemyCycle());
        BeginCorounine(DifficultyRaise());
    }

    public void UpdateValletUI()
    {
        UpdateVallet?.Invoke();
    }

    public void UpdateEnemiesCountUI()
    {
        UpdateEnemiesCount?.Invoke();
    }

    public void UpdateScoreUI()
    {
        UpdateScore?.Invoke();
    }

    public void BeginCorounine(IEnumerator _coroutine)
    {
        IEnumerator coroutine = _coroutine;
        CoroutinesList.Add(coroutine);
        StartCoroutine(coroutine);
    }
    public void EndCorounine(IEnumerator _coroutine)
    {
        for (int i = 0; i<CoroutinesList.Count; i++)
        {
            if (CoroutinesList[i].ToString() == _coroutine.ToString())
            {
                StopCoroutine(CoroutinesList[i]);
                CoroutinesList.RemoveAt(i);
                break;
            }
        }
    }

    IEnumerator SpawnEnemyCycle()
    {
        spawner.SpawnEnemy(EnemyLib.EnemyListsByDifficulty[DifficultyID].EnemyList[0]);
        while (true) 
        {
            SpawnDelay = UnityEngine.Random.Range(EnemyLib.EnemyListsByDifficulty[DifficultyID].MinSpawnTimeDelay, 
                                                  EnemyLib.EnemyListsByDifficulty[DifficultyID].MinSpawnTimeDelay);
            yield return new WaitForSeconds(SpawnDelay);
            spawner.SpawnEnemy(EnemyLib.EnemyListsByDifficulty[DifficultyID].EnemyList[0]);

            if(EnemiesOnField.Count >= 10)
            {
                LoseGame();
                break;
            }
            Debug.Log("spawn");
        }
    }

    IEnumerator DifficultyRaise()
    {
        UpdateDifficulty?.Invoke();
        while (DifficultyID < 2)
        {
            yield return new WaitForSeconds(40);
            DifficultyID++;
            UpdateDifficulty?.Invoke();
        }
    }

    private void LoseGame()
    {
        Time.timeScale = 0;
        Instantiate(Resources.Load<LosePanel>("Prefabs/Panels/LosePanel"),Canvas);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Cannon.gameObject.SetActive(false);

        GlobalContext.Instance.WriteScore(Score);
    }

    public IEnumerator FreezeSpawnEnemyCycle(float seconds)
    {
        EndCorounine(SpawnEnemyCycle());
        Debug.Log("stop spawn");
        yield return new WaitForSeconds(seconds);
        Debug.Log("start spawn");
        BeginCorounine(SpawnEnemyCycle());
        EndCorounine(FreezeSpawnEnemyCycle(seconds));
    }
}
