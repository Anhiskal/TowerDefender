using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField]
    Enemy prefab = default;

    [SerializeField]
    float speedEnemy = 5f;
    [SerializeField]
    float maxHeal;
    [SerializeField]
    float damage;

    public Enemy get()
    {
        Enemy instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        instance.initialize(speedEnemy, maxHeal, damage);
        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(enemy.gameObject);
    }    
}
