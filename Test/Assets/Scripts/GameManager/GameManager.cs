using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SlotMap;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Vector2Int mapSize = new Vector2Int(11, 11);

    [SerializeField]
    GameMap map = default;

    [SerializeField]
    GameSlotContentFactory tileContentFactory = default;

    [SerializeField]
    EnemyFactory enemyFactory = default;
    [SerializeField, Range(0.1f, 10f)]
    float spawnSpeed = 1f;
    float spawnProgress;
    [SerializeField]
    float timeForSpawn = 3f;

    EnemyCollection enemies = new EnemyCollection();

    [SerializeField]
    SlotGameSelection gameSelection;

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    void Awake()
    {
        map.Initialize(mapSize, tileContentFactory);        
    }

    void OnValidate()
    {
        if (mapSize.x < 2)
        {
            mapSize.x = 2;
        }
        if (mapSize.y < 2)
        {
            mapSize.y = 2;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            handleTouch();
        }        

        if (map.SpawnPointCount >= 1)
        {
            spawnProgress += spawnSpeed * Time.deltaTime;
            while (spawnProgress >= timeForSpawn)
            {
                spawnProgress = 0;
                spawanEnemy();
            }
        }
        if (enemies.enemiesCount >= 1 ) 
        {
            enemies.GameUpdate();
        }

        Physics.SyncTransforms();
        map.gameUpdate();
    }

    void handleTouch()
    {
        SlotMap slot = map.getSlot(TouchRay);
        if (slot != null)
        {
            //map.ToggleTower(slot);
            changeSlotMap(slot);
        }
    }
    private void changeSlotMap(SlotMap slot)
    {        
        switch (gameSelection.gameSlotContentType) 
        {
            case GameSlotContentType.Empty: map.ToggleEmpty(slot); break;
            case GameSlotContentType.Destination: map.ToggleDestination(slot); break;
            case GameSlotContentType.Wall: map.ToggleWall(slot); break;
            case GameSlotContentType.Tower: map.ToggleTower(slot); break;             
            case GameSlotContentType.SpawnPoint: map.ToggleSpawnPoint(slot); break;
        }
    }

    void spawanEnemy() 
    {
        SlotMap spawnPoint = map.getSpawnPoint(Random.Range(0, map.SpawnPointCount));
        Enemy enemy = enemyFactory.get();
        enemy.spawnOn(spawnPoint);
        enemies.Add(enemy);
    }   
}
