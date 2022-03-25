using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioClip audioClip;

    public AudioSource audiosrc;

    void Start()
    {
        audiosrc.volume = 1f;
    }

    public void Sounds(string clip)
    {
        switch (clip)
        {
            case "audioClip":
                audiosrc.PlayOneShot(audioClip);
                break;
        }
    }
}
