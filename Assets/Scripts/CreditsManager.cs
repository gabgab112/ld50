using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] string levelToLoad = "MainMenu";
    bool oneTime;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("E") && !oneTime)
        {
            bl_SceneLoaderUtils.GetLoader.LoadLevel(levelToLoad);
            oneTime = true;
        }
    }
}
