using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("State")]
    public GameState gameState;
    public enum GameState { Playing, Pause, GameOver };

    [Header("Scene")]
    public string currentScene;
    public string lastLevelName = "Level35";
    public int currentLevelID;

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

        if (currentScene != "MainMenu")
        {
            gameState = GameState.Playing;

            // Get Current Level
            //currentLevelID = Int32.Parse(currentScene.Remove(0, 5));

            //SoundManager.Instance.Sounds("stop");

            //GetComponents();

            UIManager.Instance.SetStartUI();
        }

        //SoundManager.Instance.ChangeSoundtrackByLevel();
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
