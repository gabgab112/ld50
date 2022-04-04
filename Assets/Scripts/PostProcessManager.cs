using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessManager : MonoBehaviour
{
    [Header("Zones")]
    public Zones zones;
    public enum Zones { Volcano, Forest, City };

    [SerializeField] string title;

    Volume ppOld;
    Volume ppNew;
    bool oneTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameManager.Instance != null)
        {
            if (collision.CompareTag("Player") && !oneTime)
            {
                oneTime = true;

                if (!GameManager.Instance.shownTitles.Contains(title))
                {
                    GameManager.Instance.shownTitles.Add(title);
                    UIManager.Instance.ShowTitle(title);
                }

                // Zones
                if (zones == Zones.Forest)
                {
                    ppOld = GameManager.Instance.levelManager.postProcessVolcano;
                    ppNew = GameManager.Instance.levelManager.postProcessForest;
                    GameManager.Instance.zones = GameManager.Zones.Forest;
                }
                else if (zones == Zones.City)
                {
                    ppOld = GameManager.Instance.levelManager.postProcessForest;
                    ppNew = GameManager.Instance.levelManager.postProcessCity;
                    GameManager.Instance.zones = GameManager.Zones.City;
                }

                StartCoroutine(ChangeOldPP(1f, 0f, 2f));
                StartCoroutine(ChangeNewPP(0f, 1f, 2f));

                // Music
                SoundManager.Instance.ChangeSoundtrackByZone();
            }
        }
    }

    IEnumerator ChangeOldPP(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            ppOld.weight = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ppOld.weight = v_end;
    }

    IEnumerator ChangeNewPP(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            ppNew.weight = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ppNew.weight = v_end;
    }
}
