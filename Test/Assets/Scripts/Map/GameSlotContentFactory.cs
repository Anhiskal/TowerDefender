using UnityEngine;
using UnityEngine.SceneManagement;
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
    Tower towerPrefab = default;

    [SerializeField]
    GameSlotContent spawnPrefab = default;

    Scene contentScene;

    public void reclaim(GameSlotContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject);
    }

    GameSlotContent get(GameSlotContent prefab)
    {
        GameSlotContent instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        MoveToFactoryScene(instance.gameObject);

        return instance;
    }

    void MoveToFactoryScene(GameObject o)
    {
        if (!contentScene.isLoaded)
        {
            if (Application.isEditor)
            {
                contentScene = SceneManager.GetSceneByName(name);
                if (!contentScene.isLoaded)
                {
                    contentScene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                contentScene = SceneManager.CreateScene(name);
            }
        }
        SceneManager.MoveGameObjectToScene(o, contentScene);
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
