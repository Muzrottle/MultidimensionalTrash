using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WeaponController weaponController;

    float spawnTime = 10f;

    public GameObject gameLostTxt;

    public GameObject enemy;
    public GameObject enemySpawn1;
    public GameObject enemySpawn2;
    public GameObject enemySpawn3;
    public GameObject enemySpawn4;
    public GameObject enemySpawn5;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    { 
        if (spawnTime % 5 == 0)
        {
            int randomNumber = Random.Range(1, 6);

            if (randomNumber == 1)
            {
                Instantiate(enemy, enemySpawn1.transform.position, enemySpawn1.transform.rotation);
            }
            else if (randomNumber == 2)
            {
                Instantiate(enemy, enemySpawn2.transform.position, enemySpawn2.transform.rotation);
            }
            else if (randomNumber == 3)
            {
                Instantiate(enemy, enemySpawn3.transform.position, enemySpawn3.transform.rotation);
            }
            else if (randomNumber == 4)
            {
                Instantiate(enemy, enemySpawn4.transform.position, enemySpawn4.transform.rotation);
            }
            else if (randomNumber == 5)
            {
                Instantiate(enemy, enemySpawn5.transform.position, enemySpawn5.transform.rotation);
            }
        }
        yield return new WaitForSeconds(spawnTime);
        StartCoroutine(Spawner());
    }

    public void GameLost()
    {
        gameLostTxt.SetActive(true);
        weaponController.IsInputEnabled = false;
    }
}
