using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject playPanel;
    public GameObject optionsPanel;
    public GameObject secondPanel;
    public Animator UIAnimator;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hover;
    [SerializeField] AudioClip click;
    [SerializeField] AudioClip back;
    [SerializeField] AudioClip special;

    void Start()
    {
        homePanel.SetActive(true);
        playPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

    public void LoadLevel(string levelToLoad)
    {
        // Load level
        bl_SceneLoaderUtils.GetLoader.LoadLevel(levelToLoad);
    }

    public void HideSecondPanel()
    {
        secondPanel.SetActive(false);
        UIAnimator.SetTrigger("Show");
    }

    public void Quit()
    {
        Application.Quit();
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
}
