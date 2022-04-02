using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Body : MonoBehaviour
{
    [SerializeField] Player player;
    bool oneTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && !oneTime)
        {
            oneTime = true;
            UIManager.Instance.HurtUI();
            player.Die();
        }
    }
}
