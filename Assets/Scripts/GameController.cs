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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
