using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    public GameManager gameManager;

    PlayerController playerScript;

    bool test;

    void Start()
    {
        test = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerScript = gameManager.player.GetComponent<PlayerController>();
    }
        void OnTriggerEnter(Collider other)
    {
        // Check collision between a bullet and an enemy
        if (this.tag == "Bullet")
        {
            if (other.tag == "Enemy"){
                test = true;

                EnemyController enemyScript = other.gameObject.GetComponent<EnemyController>();

                float dist = Vector3.Distance(enemyScript.GetWeakPoint(), transform.position - other.transform.position);

                if (dist <= 0.5f)
                {
                    // Critical Hit
                    /*
                    gameManager.UpdateScore(gameManager.scoreEnemyHit, true);
                    enemyScript.UpdateHealth(-playerScript.damage, true);
                    */
                    Destroy(other.gameObject);
                    Destroy(this.gameObject);
                }
                else
                {
                    /*
                    gameManager.UpdateScore(gameManager.scoreEnemyHit, false);
                    enemyScript.UpdateHealth(-playerScript.damage, false);
                    */
                    Destroy(other.gameObject);
                    Destroy(this.gameObject);
                }
            }
            if (other.tag == "Player")
            {
                //The player has been hit
                Destroy(other.gameObject);
            }
            if(other.tag == "Bullet")
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }


        } else if (this.tag == "Player")
        {
            if(other.tag == "Enemy")
            {
                //This should cause an explosion, for now it means destroying both
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }else if(this.tag=="Enemy" && other.tag =="Bullet")
        {
            test = true;
        }
        //Since detecting collision works both ways, we dont need to create reciprocal if statements for the enemy (all combinations are already handled)
    }

    private void Update()
    {
    }

    private void OnGUI()
    {
        GUI.color = Color.white;
        GUI.skin.box.fontSize = 15;
        GUI.skin.box.wordWrap = false;
        GUI.Box(new Rect(0, 60, 300, 30), "Testing typing capabilities");

        if (test)
        {
            GUI.Box(new Rect(0, 30, 300, 30), "REGISTERED A HIT!");
        }
    }
}
