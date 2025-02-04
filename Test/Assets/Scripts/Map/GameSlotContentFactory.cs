using UnityEngine;
using static SlotMap;

[CreateAssetMenu]
public class GameSlotContentFactory : GameObjectFactory
{
    [SerializeField]
    GameSlotContent destinationPrefab = default;

    [SerializeField]
    GameSlotContent emptyPrefab = default;

    [SerializeField]
    GameSlotContent wallPrefab = default;

    [SerializeField]
    GameSlotContent towerPrefab = default;

    [SerializeField]
    GameSlotContent spawnPrefab = default;    

    public void reclaim(GameSlotContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject);
    }

    GameSlotContent get(GameSlotContent prefab)
    {
        GameSlotContent instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        
        return instance;
    }

    public GameSlotContent get(GameSlotContentType type)
    {
        switch (type)
        {
            case GameSlotContentType.Destination: return get(destinationPrefab);
            case GameSlotContentType.Empty: return get(emptyPrefab);
            case GameSlotContentType.Wall: return get(wallPrefab);
            case GameSlotContentType.Tower: return get(towerPrefab);
            case GameSlotContentType.SpawnPoint: return get(spawnPrefab);
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }
}
