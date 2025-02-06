using System;
using System.Collections.Generic;

[System.Serializable]
public class EnemyCollection
{
    List<Enemy> enemies = new List<Enemy>();
    bool enemyDeathInBase;

    Action aEnemyDeathInbase, aEnemyDeathForTowers;

    public void initeCallBacks(Action deathInBase, Action deathForTower) 
    {
        aEnemyDeathInbase = deathInBase;
        aEnemyDeathForTowers = deathForTower;
    }

    public void Add(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].gameUpdate())
            {
                int lastIndex = enemies.Count - 1;
                enemies[i] = enemies[lastIndex];
                enemyDeathInBase = enemies[i].IsTouchTheBase;
                enemies.RemoveAt(lastIndex);
                i -= 1;
                if (enemyDeathInBase) 
                {
                    enemyDeathInbase();
                }
                else 
                {
                    enemyDeathForTowers();
                }
            }
        }
    }

    public int enemiesCount => enemies.Count;

    void enemyDeathInbase() 
    {
        aEnemyDeathInbase.Invoke();
    }

    void enemyDeathForTowers() 
    {
        aEnemyDeathForTowers.Invoke();
    }
}