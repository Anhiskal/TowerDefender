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

    [SerializeField, FloatRangeSlider(0.5f, 2f)]
    FloatRange scale = new FloatRange(1f);

    [SerializeField, FloatRangeSlider(-0.4f, 0.4f)]
    FloatRange pathOffset = new FloatRange(0f);

    [SerializeField, FloatRangeSlider(0.2f, 5f)]
    FloatRange speed = new FloatRange(1f);

    public Enemy get()
    {
        Enemy instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        instance.initialize(speed.RandomValueInRange, maxHeal, damage);
        instance.Initialize(scale.RandomValueInRange, pathOffset.RandomValueInRange);
        return instance;
    }

    public void Reclaim(Enemy enemy)
    {
        Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(enemy.gameObject);
    }    
}
