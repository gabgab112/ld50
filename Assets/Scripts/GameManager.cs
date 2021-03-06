using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("State")]
    public GameState gameState;
    public enum GameState { Start, Playing, Pause, GameOver };

    [Header("Gameplay")]
    //public int money;
    public int currentDistance;
    public int recordDistance;
    public float waterBonus = 0.5f;
    public float volcanoProgress;

    [Header("Zones")]
    public Zones zones;
    public enum Zones { Volcano, Forest, City };

    [Header("Scene")]
    public string currentScene;
    public List<string> notALevel = new List<string>();

    public List<string> shownTitles = new List<string>();

    [Header("Volume")]
    public float defaultVolume = 0.9f;
    public float currentVolume;

    [HideInInspector] public LevelManager levelManager;

    // Singleton instantation
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (GameObject.Find("GameManager_instance")) Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "GameManager_instance";
    }

    void Update()
    {
        if (Input.GetButtonDown("R"))
            Restart();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;

        //if (Player.Instance != null)
        //    Destroy(Player.Instance.gameObject);

        if (UIManager.Instance != null)
            Destroy(UIManager.Instance.gameObject);

        bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
        Destroy(gameObject);
    }

    public void CursorLock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CursorUnlock()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;

        if (!notALevel.Contains(currentScene))
        {
            StartCoroutine(StartGame());

            GetComponents();
            recordDistance = PlayerPrefs.GetInt("HighScore");

            // Level
            currentDistance = 0;
            zones = Zones.Volcano;

            // UI
            UIManager.Instance.SetStartUI();

            // Sound
            SoundManager.Instance.ChangeSoundtrackByZone();
        }
        else
        {
            // Not a level
        }
    }

    private void GetComponents()
    {
        if (GameObject.FindGameObjectWithTag("LevelManager"))
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    IEnumerator StartGame()
    {
        gameState = GameState.Start;
        yield return new WaitForSeconds(3f);
        gameState = GameState.Playing;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
}
