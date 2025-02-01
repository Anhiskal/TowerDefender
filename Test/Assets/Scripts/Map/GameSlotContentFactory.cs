using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SlotMap;

[CreateAssetMenu]
public class GameSlotContentFactory : ScriptableObject
{
    [SerializeField]
    GameSlotContent destinationPrefab = default;

    [SerializeField]
    GameSlotContent emptyPrefab = default;

    [SerializeField]
    GameSlotContent wallPrefab = default;

    [SerializeField]
    GameSlotContent towerPrefab = default;

    Scene contentScene;

    public void reclaim(GameSlotContent content)
    {
        Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(content.gameObject);
    }

    GameSlotContent get(GameSlotContent prefab)
    {
        GameSlotContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        moveToFactoryScene(instance.gameObject);
        return instance;
    }

    void moveToFactoryScene(GameObject o)
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
        }
        Debug.Assert(false, "Unsupported type: " + type);
        return null;
    }
}
