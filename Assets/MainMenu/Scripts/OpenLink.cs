using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public void OpenWebsite()
    {
        Application.OpenURL("https://www.gabrielbissonnette.com/");
    }

    public void OpenAnotherLink(string url)
    {
        Application.OpenURL(url);
    }
}
