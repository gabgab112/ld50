using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Values")]
    public float momentum;
    public float maxSpeed;
    public float minSpeed;
    float originalSpeed;
    public float gravity = 20.0f;
    public float jumpHeight = 2.5f;
    float jumpMomentum;

    [Header("Game Over Conditions")]
    public float delayBeforeGameOver;
    public float minVelocity;
    public float maxVelocity;

    [Header("Skate")]
    public GameObject skate;
    public CapsuleCollider2D skateCollider;
    //public Rigidbody2D skateRb;

    [Header("Character")]
    //public CapsuleCollider2D afterDeathCollider;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    public GameObject playerSprite;

    bool grounded = false;
    Vector3 defaultScale;
    bool crouch = false;
    bool jumpRamp;
    bool trickshotBoost;
    bool oneTimeGameOver;

    float rotationTimerLeft;
    float rotationTimerRight;
    bool rotate;
    public float strength = 50f;

    void Start()
    {
        defaultScale = playerSprite.transform.localScale;
    }

    void Update()
    {
        // Game Over
        if(GameManager.Instance.gameState == GameManager.GameState.Playing)
        {
            if (rb.velocity.x <= minVelocity)
            {
                if(!oneTimeGameOver)
                {
                    Die();
                    oneTimeGameOver = true;
                }
            }
        }

        // distance
        GameManager.Instance.currentDistance = Mathf.Max(0, Mathf.FloorToInt(transform.position.x / 2));
        UIManager.Instance.UpdateDistance();

        // Jump
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && grounded)
        {
            if(jumpRamp)
            {
                if(rb.velocity.x < maxVelocity)
                {
                    jumpMomentum = 1.25f;
                }
                else
                {
                    jumpMomentum = 1f;
                }
                
                rb.velocity = new Vector3(rb.velocity.x * jumpMomentum, CalculateJumpVerticalSpeed() / 15);
            }
            else
            {
                jumpMomentum = 1;
                rb.velocity = new Vector3(rb.velocity.x * jumpMomentum, CalculateJumpVerticalSpeed());
            }
        }

        if (!jumpRamp && !trickshotBoost)
        {
            momentum = minSpeed;
        }

        if ((Input.GetButton("Left") && !grounded))
        {
            rotate = true;            

            rb.AddTorque(120 * Time.deltaTime * strength);

            rotationTimerLeft += Time.deltaTime;
        }
        else if(Input.GetButton("Right") && !grounded)
        {
            rotate = true;

            rb.AddTorque(-120 * Time.deltaTime * strength);

            rotationTimerRight += Time.deltaTime;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("JumpRamp"))
        {
            jumpRamp = true;
        }
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
        }
        
        yield return new WaitForSeconds(1.2f);
        momentum = minSpeed;
        trickshotBoost = false;
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(delayBeforeGameOver);
        UIManager.Instance.GameOverUI();
        //GameManager.Instance.Restart();
    }

    public void Die()
    {
        //skate.transform.parent = null;

        StartCoroutine(RestartGame());
    }

    void FixedUpdate()
    {
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

                    if(rotate)
                    {
                        if (rotationTimerLeft >= 0.6f || rotationTimerRight >= 0.6f)
                        {
                            // Boost
                            //print("boost");
                            StartCoroutine(SuccessfulTrickshot());
                        }

                        rotationTimerLeft = 0;
                        rotationTimerRight = 0;

                        rotate = false;
                    }
                    
                    break;
                }
            }
        }

        rb.AddForce(new Vector3(rb.velocity.x * momentum, -gravity * rb.mass, 0));
    }

    float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2 * jumpHeight * jumpMomentum * gravity);
    }
}