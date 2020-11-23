using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    
    public Transform[] target;
    public float speed;
    public bool toggleSpeedchange = true;
    bool invokeOnce = false;

    private int current;

   

    // Update is called once per frame
    void Update()
    {
       
            if (transform.position != target[current].position)
            {
                Vector2 pos = Vector2.MoveTowards(transform.position, target[current].position, speed * Time.deltaTime);
                GetComponent<Rigidbody2D>().MovePosition(pos);
            }
            else
            {

                if (gameObject.tag == "Enemy")
                    current = (current + 1) % target.Length;

            }


            if (toggleSpeedchange == true)
            {
                if (!invokeOnce)
                {
                    StartCoroutine("cycleSpeed");
                    invokeOnce = true;
                }

            }
            else
            {
                if (invokeOnce)
                {
                    StopCoroutine("cycleSpeed");
                    invokeOnce = false;
                }
            }
    }
      
    

    IEnumerator cycleSpeed()
    {

        yield return new WaitForSeconds(3f);

        while (toggleSpeedchange)
        {
            speed += 2;
            yield return new WaitForSeconds(3f);
            speed -= 2;
            yield return new WaitForSeconds(3f);
        }
    }

}
