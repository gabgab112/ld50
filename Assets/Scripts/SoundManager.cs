using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    [Header("UI")]
    public AudioClip hover;
    public AudioClip click;
    public AudioClip back;
    public AudioClip special;

    [Header("Level")]
    public AudioClip coin;
    public AudioClip boost;
    public AudioClip jump;
    public AudioClip landing;

    [Header("Death")]
    public AudioClip death1;
    public AudioClip death2;
    public AudioClip death3;
    public AudioClip death4;
    public AudioClip die;

    //[Header("Player")]
    //public AudioClip test;

    //[Header("Death")]
    //public AudioClip test;

    [Header("Soundtracks")]
    public AudioSource[] soundtracks;

    [Header("Audio Source")]
    public AudioSource audiosrc;
    public AudioSource skateboardSrc;
    public AudioSource volcanoSrc;

    bool firstTimeDone;
    int soundtrackToPlay = 0;

    // Instance
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<SoundManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (GameObject.Find("SoundManager_instance")) Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "SoundManager_instance";

        audiosrc.ignoreListenerPause = false;
    }


    public void ChangeSoundtrackByZone()
    {
        // Volcanosrc
        volcanoSrc.volume = 0f;

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.zones == GameManager.Zones.Volcano)
            {
                soundtrackToPlay = 0;
            }
            else if(GameManager.Instance.zones == GameManager.Zones.Forest)
            {
                soundtrackToPlay = 1;
            }
            else if(GameManager.Instance.zones == GameManager.Zones.City)
            {
                soundtrackToPlay = 2;
            }

            for (int i = 0; i < soundtracks.Length; i++)
            {
                if (soundtrackToPlay == i)
                {
                    // Found the right soundtrack
                    if (soundtracks[i].volume >= 0.1f)
                    {
                        soundtracks[i].volume = 1f;
                    }
                    else
                    {
                        StartCoroutine(ChangeSpeed(soundtracks[i], 0f, 1f, 2f));
                    }
                }
                else
                {
                    // Not the right soundtrack
                    if (soundtracks[i].volume >= 0.1f)
                        StartCoroutine(ChangeSpeed(soundtracks[i], 1f, 0f, 2f));
                    else
                        soundtracks[i].volume = 0f;
                }
            }
        }
    }

    public void MuteAllSoundtracks()
    {
        foreach(AudioSource soundtrack in soundtracks)
        {
            if(soundtrack.volume >= 0.1f)
                StartCoroutine(ChangeSpeed(soundtrack, 1f, 0f, 0.5f));
            else
                soundtrack.volume = 0f;
        }
    }

    IEnumerator ChangeSpeed(AudioSource source, float v_start, float v_end, float duration )
    {
        float elapsed = 0.0f;
        while (elapsed < duration )
        {
            source.volume = Mathf.Lerp( v_start, v_end, elapsed / duration );
            elapsed += Time.deltaTime;
            yield return null;
        }
        source.volume = v_end;
    }

    public void Sounds(string clip)
    {
        switch (clip)
        {
            case "hover":
                audiosrc.PlayOneShot(hover);
                break;
            case "click":
                audiosrc.PlayOneShot(click);
                break;
            case "back":
                audiosrc.PlayOneShot(back);
                break;
            case "special":
                audiosrc.PlayOneShot(special);
                break;


            case "coin":
                audiosrc.PlayOneShot(coin);
                break;

            case "boost":
                audiosrc.PlayOneShot(boost);
                break;

            case "jump":
                audiosrc.PlayOneShot(jump);
                break;

            case "landing":
                audiosrc.PlayOneShot(landing);
                break;

            case "die":
                audiosrc.PlayOneShot(die);
                break;


            case "death":
                int randomIndex = Random.Range(0, 4);

                if (randomIndex == 0)
                {
                    audiosrc.PlayOneShot(death1);
                }
                else if (randomIndex == 1)
                {
                    audiosrc.PlayOneShot(death2);
                }
                else if (randomIndex == 2)
                {
                    audiosrc.PlayOneShot(death3);
                }
                else if (randomIndex == 3)
                {
                    audiosrc.PlayOneShot(death4);
                }

                break;

            case "stop":
                audiosrc.Stop();
                audiosrc.Play();
                break;
        }
    }
}
