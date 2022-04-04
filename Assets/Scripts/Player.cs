using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Values")]
    public bool canMove = true;
    public bool canDie = true;
    public float momentum;
    public float maxSpeed;
    public float minSpeed;
    public float gravity = 20.0f;
    public float jumpHeight = 2.5f;
    float jumpMomentum;

    [Header("Game Over Conditions")]
    public float delayBeforeGameOver;
    public float minVelocity;
    public float maxVelocity;
    public bool isDead;

    [Header("Volcano")]
    public float volcanoConsumption = 0.0018f;
    

    [Header("Skate")]
    public GameObject skate;
    public CapsuleCollider2D skateCollider;

    [Header("Character")]
    public Animator animator;
    public CapsuleCollider2D afterDeathCollider;

    [Header("Components")]
    public Rigidbody2D rb;
    public GameObject playerSprite;
    public TrailRenderer playerTrail;

    // Movement
    bool grounded = false;
    Vector3 defaultScale;
    bool crouch = false;
    bool jumpRamp;
    bool trickshotBoost;
    bool oneTimeGameOver;

    // Rotation
    bool rotate;
    public float strength = 50f;
    int torqueForce;
    float originalAirRotation;
    bool oneTimeRecordRotation;
    float maxAngularVelocity = 230;
    float minAngularVelocity = -230;

    // Trail
    float alpha;
    float originalAlpha;

    void Start()
    {
        defaultScale = playerSprite.transform.localScale;
        canMove = true;

        alpha = 0;
        originalAlpha = playerTrail.material.color.a;

        rb.simulated = true;
        oneTimeGameOver = false;

        afterDeathCollider.enabled = false;
    }

    void Update()
    {
        if(GameManager.Instance != null)
        {
            // Game Over
            if (GameManager.Instance.gameState == GameManager.GameState.Playing && canDie)
            {
                if (rb.velocity.x <= minVelocity)
                {
                    if (!oneTimeGameOver)
                    {
                        Die();
                        oneTimeGameOver = true;
                    }
                }
            }

            


            if (canMove)
            {
                if (GameManager.Instance.gameState != GameManager.GameState.GameOver)
                {
                    // distance
                    GameManager.Instance.currentDistance = Mathf.Max(0, Mathf.FloorToInt(transform.position.x / 1.5f));
                    UIManager.Instance.UpdateDistance();
                }

                // Jump
                if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && grounded)
                {
                    if (jumpRamp)
                    {
                        if (rb.velocity.x < maxVelocity)
                        {
                            jumpMomentum = 1.25f;
                        }
                        else
                        {
                            jumpMomentum = 1f;
                        }

                        SoundManager.Instance.Sounds("boost");

                        rb.velocity = new Vector3(rb.velocity.x * jumpMomentum, CalculateJumpVerticalSpeed() / 15);
                    }
                    else
                    {
                        jumpMomentum = 1;
                        SoundManager.Instance.Sounds("jump");
                        rb.velocity = new Vector3(rb.velocity.x * jumpMomentum, CalculateJumpVerticalSpeed());
                    }
                }

                if (!jumpRamp && !trickshotBoost)
                {
                    momentum = minSpeed;
                }

                

                // Rotation
                if ((Input.GetButton("Left") && !grounded))
                {
                    rb.AddTorque(TorqueForce() * Time.deltaTime * strength);
                    rotate = true;
                }
                else if (Input.GetButton("Right") && !grounded)
                {
                    rb.AddTorque(-TorqueForce() * Time.deltaTime * strength);
                    rotate = true;
                }

                // Trail
                if (rb.velocity.x >= maxVelocity / 1.5f)
                {
                    alpha = Mathf.Lerp(playerTrail.material.color.a, originalAlpha, Time.deltaTime * 5);
                    playerTrail.material.color = new Color(1f, 1f, 1f, alpha);

                    playerTrail.time = Mathf.Lerp(playerTrail.time, 0.9f, Time.deltaTime * 10);
                }
                else
                {
                    alpha = Mathf.Lerp(playerTrail.material.color.a, 0f, Time.deltaTime * 10);
                    playerTrail.material.color = new Color(1f, 1f, 1f, alpha);
                }

                // Animations
                animator.SetBool("grounded", grounded);
                animator.SetBool("rotating", rotate);
            }
            else
            {
                //rb.velocity = new Vector2(0, 0);
                momentum = 0;
            }

            if (grounded)
            {
                // Skateboard sound
                if (!isDead)
                {
                    if (rb.velocity.x > minVelocity)
                        SoundManager.Instance.skateboardSrc.volume = 1f;
                }
                else
                {
                    SoundManager.Instance.skateboardSrc.volume = 0f;
                }
            }
            else if (!grounded)
            {
                SoundManager.Instance.skateboardSrc.volume = 0f;

                // Flips
                if (!oneTimeRecordRotation)
                {
                    originalAirRotation = rb.rotation;
                    oneTimeRecordRotation = true;
                }
            }

            //Crouch
            //crouch = Input.GetKey(KeyCode.S);
            //if (crouch && rb.velocity.x >= 8)
            //{
            //    momentum = maxSpeed / 2;
            //    playerSprite.transform.localScale = Vector3.Lerp(playerSprite.transform.localScale, new Vector3(defaultScale.x, defaultScale.y * 0.4f, defaultScale.z), Time.deltaTime * 7);
            //}
            //else
            //{
            //    if(!jumpRamp)
            //    {
            //        momentum = minSpeed;
            //    }

            //    playerSprite.transform.localScale = Vector3.Lerp(playerSprite.transform.localScale, defaultScale, Time.deltaTime * 7);
            //}
        }
    }

    int TorqueForce()
    {
        //if (rb.velocity.magnitude <= 10)
        //{
        //    torqueForce = 110;
        //}
        //else if (rb.velocity.magnitude <= 20)
        //{
        //    torqueForce = 90;
        //}
        //else if (rb.velocity.magnitude <= 30)
        //{
        //    torqueForce = 100;
        //}
        //else
        //{
        //    torqueForce = 100;
        //}
        torqueForce = 120;
        return torqueForce;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("JumpRamp"))
        {
            jumpRamp = true;
        }
        else if (collision.CompareTag("DeathZone") && !oneTimeGameOver)
        {
            oneTimeGameOver = true;
            Die();
        }
        else if (collision.CompareTag("Ground") && !oneTimeGameOver)
        {
            oneTimeGameOver = true;
            UIManager.Instance.HurtUI();
            Die();
        }
        //else if(collision.CompareTag("Coin"))
        //{
        //    GameManager.Instance.money += 1;

        //    // Sound
        //    SoundManager.Instance.Sounds("coin");
        //    collision.gameObject.SetActive(false);
        //    //Destroy(collision.gameObject);
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("JumpRamp") && rb.velocity.x < maxVelocity)
        {
            momentum = maxSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("JumpRamp"))
        {
            momentum = minSpeed;
            jumpRamp = false;
        }
    }

    IEnumerator SuccessfulTrickshot()
    {
        if(rb.velocity.x < maxVelocity)
        {
            trickshotBoost = true;
            momentum = maxSpeed;

            SoundManager.Instance.Sounds("boost");
        }
        
        yield return new WaitForSeconds(1.2f);
        momentum = minSpeed;
        trickshotBoost = false;
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(delayBeforeGameOver);

        SoundManager.Instance.Sounds("die");
        UIManager.Instance.GameOverUI();
        //GameManager.Instance.Restart();
    }

    public void Die()
    {
        afterDeathCollider.enabled = true;

        rb.drag = 2;
        isDead = true;

        GameManager.Instance.gameState = GameManager.GameState.GameOver;

        SoundManager.Instance.Sounds("death");

        //rb.simulated = false;
        canMove = false;
        StartCoroutine(RestartGame());
    }

    void FixedUpdate()
    {
        // Limit rotating speed
        if (rb.angularVelocity > maxAngularVelocity && rb.angularVelocity > 0)
        {
            rb.angularVelocity = maxAngularVelocity;
        }
        else if(rb.angularVelocity < minAngularVelocity && rb.angularVelocity < 0)
        {
            rb.angularVelocity = minAngularVelocity;
        }

        if(UIManager.Instance != null)
        {
            // Volcano
            if (!isDead)
            {
                UIManager.Instance.volcanoQty.fillAmount -= volcanoConsumption * Time.fixedDeltaTime;
                GameManager.Instance.volcanoProgress = UIManager.Instance.volcanoQty.fillAmount;

                if(GameManager.Instance.volcanoProgress <= 0)
                {
                    Die();
                }

                if (SoundManager.Instance.volcanoSrc.volume < 1)
                {
                    SoundManager.Instance.volcanoSrc.volume += volcanoConsumption * Time.fixedDeltaTime;
                }
            }
            else
            {
                //UIManager.Instance.volcanoQty.fillAmount -= GameManager.Instance.volcanoProgress;
            }
        }


        Bounds colliderBounds = skateCollider.bounds;
        float colliderRadius = skateCollider.size.x * 0.4f * Mathf.Abs(skateCollider.transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        grounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != skateCollider && colliders[i].gameObject.layer == 6)
                {
                    grounded = true;

                    if (rotate)
                    {
                        if (originalAirRotation + 180 <= rb.rotation)
                        {
                            //print("made a flip left");
                            StartCoroutine(SuccessfulTrickshot());
                        }
                        else if (originalAirRotation - 180 >= rb.rotation)
                        {
                            //print("made a flip right");
                            StartCoroutine(SuccessfulTrickshot());
                        }
                        else
                        {
                            //print($"originalAirRotation {originalAirRotation} + 180 <= rb.rotation {rb.rotation}");
                        }

                        SoundManager.Instance.Sounds("landing");

                        oneTimeRecordRotation = false;
                        rotate = false;
                    }

                    break;
                }
            }
        }

        if(!isDead)
            rb.AddForce(new Vector3(rb.velocity.x * momentum, -gravity * rb.mass, 0));
    }

    float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2 * jumpHeight * jumpMomentum * gravity);
    }
}