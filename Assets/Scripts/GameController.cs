using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int lives;
    private int score;
    [Header("Game Objects")]
    public GameObject coins;
    public int coinsCount;
    public Text scoreLabel;
    public Text livesLabel;
    [Header("Sounds")]
    public AudioSource enemySound;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreLabel.text = score.ToString() + " / " + coinsCount.ToString();
        }
    }

    public int Lives
    {
        get { return lives; }
        set
        {
            lives = value;
            livesLabel.text = "Lives: " + lives.ToString();
            if(lives <= 0)
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        coinsCount = coins.transform.childCount;
        scoreLabel.text = "0" + " / " + coinsCount.ToString();
        Lives = 3;
    }
    public void GotHit()
    {
        //this.liveImages[lives].enabled = false;
        Lives--;
        enemySound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
