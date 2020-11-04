using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public int coins;
    public Text coinCountText;

    void Start()
    {
        coins = 0;
    }


    void Update()
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Coins:" + coins);
            if (other.tag == "Coin") coins++;
            Destroy(other.gameObject);
            coinCountText.text = "Score: " + coins;
        }

    }
}
