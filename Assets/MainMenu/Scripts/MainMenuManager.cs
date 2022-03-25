using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject playPanel;
    public GameObject optionsPanel;

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

    public void Quit()
    {
        Application.Quit();
    }
}
