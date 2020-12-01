using System.Collections;
using System.Collections.Generic;
using Util;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour
{
    [Header("Player Objects")]
    public Animator anim2; // Is used to store Animator component of the hero 
    public Player2AnimState hero2AnimState; // Is used to manage hero animation state
    public SpriteRenderer spriteRenderer2; // Is used to store SpriteRendere component of the hero
    [Header("Movement Values")]
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public float speed2; // Is used to store speed of the hero movement
    public float jumpForce; // Is used to store jump force of the hero
    [Header("Maximum Velocity")]
    public Vector2 maxVel = new Vector2(); // Is used to set maximum velocity of the hero(is used to manage jumping and moving of the hero)
    //public GameController gameController; // Is used to store GameController object
    private Rigidbody2D rgb2; // Is used to store Rigidbody2D component of the hero

    public bool grounded2; // Is used to represent boolean(true or false) value to check if the hero is on the ground
                          // Start is called before the first frame update

    [Header("Scoring")]
    public GameController gameController;
    [Header("Sounds")]
    public AudioSource coinSound;
    public AudioSource jumpSound;
    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        rgb2 = GetComponent<Rigidbody2D>(); // Is used to get Rigidbody2D component
        //heroAnimState = HeroAnimState.IDLE; // Sets animation of the hero to Idle
        //coins = 0; // counting score
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector2.down * 0.1f, Color.red);
        // Move is used to move player
        CheckIfGrounded2();
        Move2();
    }
    
    /// <summary>
    /// This method is responsible for moving character and checikng if it is on the ground
    /// </summary>
    public void Move2()
    {
        //grounded = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0.0f, Vector2.down, 0.5f, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetKeyDown(KeyCode.RightArrow) && (grounded2))
        {
            spriteRenderer2.flipX = false;
            hero2AnimState = Player2AnimState.WALK;
            anim2.SetInteger("AnimState", (int)Player2AnimState.WALK);
            rgb2.AddForce(Vector2.right * speed2);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && (grounded2))
        {
            spriteRenderer2.flipX = true;
            hero2AnimState = Player2AnimState.WALK;
            anim2.SetInteger("AnimState", (int)Player2AnimState.WALK);
            rgb2.AddForce(Vector2.left * speed2);
        }

        if (Input.GetAxis("Horizontal") == 0 && (grounded2))
        {
            hero2AnimState = Player2AnimState.IDLE;
            anim2.SetInteger("AnimState", (int)Player2AnimState.IDLE);
        }
        if (Input.GetKeyDown(KeyCode.RightShift) && (grounded2))
        {
            jumpSound.Play();
            anim2.SetInteger("AnimState", (int)Player2AnimState.JUMP);
            hero2AnimState = Player2AnimState.JUMP;
            rgb2.AddForce(new Vector2(0.0f, jumpForce));
            grounded2 = false;
        }
        rgb2.velocity = new Vector2(Input.GetAxis("Horizontal") * speed2, rgb2.velocity.y);
        //rgb.velocity = new Vector2(Mathf.Clamp(rgb.velocity.x, -maxVel.x, maxVel.x), Mathf.Clamp(rgb.velocity.y, -maxVel.y, maxVel.y));
    }
    /// <summary>
    /// This method is used to reset player's position
    /// </summary>
    public void Reset()
    {
        this.transform.position = new Vector2(0f, -9f);
    }

    void CheckIfGrounded2()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            grounded2 = true;
        }
        else
        {
            grounded2 = false;
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
                if (gameController.Score == gameController.coinsCount)
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
                Reset();
                break;
        }
    }
}
