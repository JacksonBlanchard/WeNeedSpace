using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    public GameManager gameManager;

    PlayerController playerScript;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerScript = gameManager.player.GetComponent<PlayerController>();
    }
    void OnTriggerEnter(Collider other)
    {
        // Check collision between a bullet and an enemy
        if (this.tag == "Bullet")
        {
            if (other.tag == "Enemy")
            {
                //This line isnt necessary, as the enemies all die in one hit currently so no health mechanics, no need for script

                //EnemyController enemyScript = other.gameObject.GetComponent<EnemyController>();

                Destroy(other.gameObject);
                Destroy(this.gameObject);
                gameManager.IncrementKill();
                gameManager.UpdateScore(gameManager.scoreEnemyHit);
                /*
                enemyScript.UpdateHealth(-playerScript.damage);
                */
            }
            else if (other.tag == "Bullet")
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
            else if (other.tag == "Wall")
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
        else if (this.tag == "Player")
        {
            if (other.tag == "Enemy")
            {
                //This should cause an explosion, for now it means destroying the enemy
                Destroy(other.gameObject);
                // Destroy(this.gameObject);
            }
        }
        //Since detecting collision works both ways, we dont need to create reciprocal if statements for the enemy (all combinations are already handled)
    }

    private void Update()
    {
    }
}
