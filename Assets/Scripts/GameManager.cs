using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public PlayerController PlayerController;

    public List<GameObject> EnemiesToInstantiate;
    public int CantidadZombiesPorHorda = 10;
    public float SpawnRadius = 5f;

    public static GameManager Instance { private set; get; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0 ; i < CantidadZombiesPorHorda; i++)
        {
            var instantiatePosition = new Vector3(
                UnityEngine.Random.Range(Player.position.x + SpawnRadius, Player.position.x - SpawnRadius),
                0f,
                UnityEngine.Random.Range(Player.position.z + SpawnRadius, Player.position.z - SpawnRadius)
            );
            int random = UnityEngine.Random.Range(0,2);
            var enemy = Instantiate(EnemiesToInstantiate[random], instantiatePosition, Quaternion.identity);
            enemy.GetComponent<EnemyController>().Player = GameManager.Instance.Player;
            enemy.GetComponent<EnemyController>().playerController = GameManager.Instance.PlayerController;
        }
        
    }
}
