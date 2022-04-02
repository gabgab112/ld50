using PixelsoftGames.PixelUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject fadeIn;
    public GameObject gameOverPanel;
    public GameObject hurt;
    [Space(20)]

    [Header("HUD")]
    [SerializeField] TextMeshProUGUI inputText;
    [SerializeField] UITypewriter actionInputTyping;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI distanceText;

    [Space(20)]

    [Header("Pause")]
    public VolumeSlider volumeSlider;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hover;
    [SerializeField] AudioClip click;
    [SerializeField] AudioClip back;
    [SerializeField] AudioClip special;

    public bool isPointerOverPauseButton;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<UIManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (GameObject.Find("UIManager_instance")) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "UIManager_instance";

        audioSource.ignoreListenerPause = true;
        AudioListener.pause = false;
    }

    public void SetStartUI()
    {
        Time.timeScale = 1;

        //actionInputTyping = inputText.GetComponent<UITypewriter>();

        //UpdateInputText("");
        SetEmptyInputText();
        UpdateMoney();

        fadeIn.SetActive(true);

        // HUD
        if(!GameManager.Instance.notALevel.Contains(GameManager.Instance.currentScene))
        {
            hurt.SetActive(false);
            hudPanel.SetActive(true);
            pausePanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }
        else
        {
            hurt.SetActive(false);
            hudPanel.SetActive(false);
            pausePanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }
        

        isPointerOverPauseButton = false;

        StartCoroutine(HideFadeIn());
    }

    IEnumerator HideFadeIn()
    {
        yield return new WaitForSeconds(3f);
        fadeIn.SetActive(false);
    }


    #region Pause
    public void Pause()
    {
        pausePanel.SetActive(true);
        hudPanel.SetActive(false);

        // CrazyGames
        //CrazyEvents.Instance.GameplayStop();

        GameManager.Instance.gameState = GameManager.GameState.Pause;

        //volumeSlider.SetStartVolume();

        AudioListener.pause = true;
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        GameManager.Instance.gameState = GameManager.GameState.Playing;


        // CrazyGames
        //CrazyEvents.Instance.GameplayStart();

        isPointerOverPauseButton = false;
        pausePanel.SetActive(false);
        hudPanel.SetActive(true);
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

    public void PointerEnterPauseButton()
    {
        isPointerOverPauseButton = true;
    }

    public void PointerExitPauseButton()
    {
        isPointerOverPauseButton = false;
    }

    public void ReturnToMenu()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;

        //if(Player.Instance != null)  
        //    Destroy(Player.Instance.gameObject);

        if(GameManager.Instance != null)
            Destroy(GameManager.Instance.gameObject);

        bl_SceneLoaderUtils.GetLoader.LoadLevel("MainMenu");
        Destroy(gameObject);
    }
    #endregion

    public void HurtUI()
    {
        hurt.SetActive(true);
    }

    public void GameOverUI()
    {
        hurt.SetActive(false);
        hudPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }


    public void RestartGame()
    {
        GameManager.Instance.Restart();
    }


    #region Buttons Sounds
    public void Hover()
    {
        audioSource.PlayOneShot(hover);
    }

    public void Click()
    {
        audioSource.PlayOneShot(click);
    }

    public void Back()
    {
        audioSource.PlayOneShot(back);
    }

    public void SpecialButton()
    {
        audioSource.PlayOneShot(special);
    }
    #endregion


    #region Update Texts
    public void UpdateInputText(string _text)
    {
        //inputText.text = _text;
        actionInputTyping.SetText(_text);
    }

    public void SetEmptyInputText()
    {
        inputText.text = "";

        if(actionInputTyping.gameObject.activeInHierarchy)
            actionInputTyping.SetText("");
    }

    public void UpdateMoney()
    {
        moneyText.text = GameManager.Instance.money.ToString() + "$";
    }

    public void UpdateDistance()
    {
        distanceText.text = GameManager.Instance.currentDistance.ToString() + "m";
    }
    #endregion

}
