using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameManager.Instance != null)
        {
            if (collision.CompareTag("Player"))
            {
                //GameManager.Instance.money += 1;

                UIManager.Instance.volcanoQty.fillAmount += GameManager.Instance.waterBonus;
                SoundManager.Instance.volcanoSrc.volume -= GameManager.Instance.waterBonus;

                // Sound
                SoundManager.Instance.Sounds("coin");

                Destroy(gameObject);
            }
        }
    }
}