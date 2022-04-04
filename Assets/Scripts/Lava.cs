using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] Player player;

    float speed = 5f;
    float originalSpeed;

    private void Start()
    {
        originalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.rb.velocity.x >= 10)
        {
            //speed = 0.03f;
            speed = 5f;
        }
        else if(player.rb.velocity.x >= 5)
        {
            //speed = 0.3f;
            speed = 5f;
        }
        else
        {
            //speed = 0.9f;
            speed = 2f;
        }

        transform.Translate(Vector3.right * player.rb.velocity.x * 0.7f * Time.deltaTime);
    }
}
