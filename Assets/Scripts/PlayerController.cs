using System.Collections;
using System.Collections.Generic;
using Util;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    

    [Header("Player Objects")]
    public Animator anim; // Is used to store Animator component of the hero 
    public PlayerAnimState heroAnimState; // Is used to manage hero animation state
    public SpriteRenderer spriteRenderer; // Is used to store SpriteRendere component of the hero
    [Header("Movement Values")]
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public float speed; // Is used to store speed of the hero movement
    public float jumpForce; // Is used to store jump force of the hero
    [Header("Maximum Velocity")]
    public Vector2 maxVel = new Vector2(); // Is used to set maximum velocity of the hero(is used to manage jumping and moving of the hero)
    //public GameController gameController; // Is used to store GameController object
    private Rigidbody2D rgb; // Is used to store Rigidbody2D component of the hero

    public bool grounded; // Is used to represent boolean(true or false) value to check if the hero is on the ground
                          // Start is called before the first frame update

    [Header("Scoring")]
    public GameController gameController;
    [Header("Sounds")]
    public AudioSource coinSound;
    public AudioSource jumpSound;
    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        rgb = GetComponent<Rigidbody2D>(); // Is used to get Rigidbody2D component
        //heroAnimState = HeroAnimState.IDLE; // Sets animation of the hero to Idle
        //coins = 0; // counting score
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector2.down * 0.1f, Color.red);
        // Move is used to move player
        CheckIfGrounded();
        Move();
    }

    /// <summary>
    /// This method is responsible for moving character and checikng if it is on the ground
    /// </summary>
    public void Move()
    {
        //grounded = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0.0f, Vector2.down, 0.5f, 1 << LayerMask.NameToLayer("Ground"));


        if (Input.GetKeyDown(KeyCode.D) && (grounded))
        {
            spriteRenderer.flipX = false;
            heroAnimState = PlayerAnimState.WALK;
            anim.SetInteger("AnimState", (int)PlayerAnimState.WALK);
            rgb.AddForce(Vector2.right * speed);
        }
        if (Input.GetKeyDown(KeyCode.A) && (grounded))
        {
            spriteRenderer.flipX = true;
            heroAnimState = PlayerAnimState.WALK;
            anim.SetInteger("AnimState", (int)PlayerAnimState.WALK);
            rgb.AddForce(Vector2.left * speed);
        }

        if (Input.GetAxis("P1_Horizontal") == 0 && (grounded))
        {
            heroAnimState = PlayerAnimState.IDLE;
            anim.SetInteger("AnimState", (int)PlayerAnimState.IDLE);
        }
        if ((Input.GetAxis("P1_Jump") > 0) && (grounded))
        {
            jumpSound.Play();
            anim.SetInteger("AnimState", (int)PlayerAnimState.JUMP);
            heroAnimState = PlayerAnimState.JUMP;
            rgb.AddForce(new Vector2(0.0f, jumpForce));
            grounded = false;
        }
        rgb.velocity = new Vector2(Input.GetAxis("P1_Horizontal") * speed, rgb.velocity.y);
        //rgb.velocity = new Vector2(Mathf.Clamp(rgb.velocity.x, -maxVel.x, maxVel.x), Mathf.Clamp(rgb.velocity.y, -maxVel.y, maxVel.y));
    }
    /// <summary>
    /// This method is used to reset player's position
    /// </summary>
    public void Reset()
    {
        this.transform.position = new Vector2(0f, -9f);
    }

    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Coin":
                collision.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(collision.gameObject);
                coinSound.Play();
                gameController.Score += 1;
                break;
            case "Transition":
                if(gameController.Score == gameController.coinsCount)
                {
                    switch (SceneManager.GetActiveScene().name)
                    {
                        case "Level1":
                            SceneManager.LoadScene("Level2");
                            Debug.Log("Suppose to do something");
                            break;
                        case "Level2":
                            SceneManager.LoadScene("Level3");
                            break;
                        case "Level3":
                            SceneManager.LoadScene("GameOver");
                            break;
                    }
                }
                
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Death_Point":
                gameController.GotHit();
                Reset();
                
                break;
            case "Enemy":
                gameController.GotHit();
                //Reset();
                break;
        }
    }
}
