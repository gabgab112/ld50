using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        SetStartVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStartVolume()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", GameManager.Instance.currentVolume);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}
