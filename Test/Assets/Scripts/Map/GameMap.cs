using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SlotMap;

public class GameMap : MonoBehaviour
{
    [SerializeField]
    Transform map = default;

    [SerializeField]
    SlotMap slotPrefab = default;

    Vector2Int size;

    SlotMap[] slots;

    GameSlotContentFactory contentFactory;

    List<SlotMap> spawnPoints = new List<SlotMap>();
    List<GameSlotContent> updatingContent = new List<GameSlotContent>();

    public void Initialize(Vector2Int size, GameSlotContentFactory contentFactory)
    {
        this.size = size;
        map.localScale = new Vector3(size.x, size.y, 1f);
        this.contentFactory = contentFactory;


        Vector2 offset = new Vector2(
            (size.x - 1) * 0.5f, (size.y - 1) * 0.5f
        );

        slots = new SlotMap[size.x * size.y];
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++,  i++)
            {
                SlotMap slot = slots[i] = Instantiate(slotPrefab);
                slot.transform.SetParent(transform, false);
                slot.transform.localPosition = new Vector3(
                    x - offset.x, 0f, y - offset.y
                );
                slot.Content = contentFactory.get(GameSlotContentType.Empty);
            }
        }

    }

    public SlotMap getSlot(Ray ray) 
    {
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1)) 
        {
            int x = (int)(hit.point.x + size.x * 0.5f);
            int y = (int)(hit.point.z + size.y * 0.5f);

            if (x >= 0 && x < size.x && y >= 0 && y < size.y) 
            {
                return slots[x + y * size.x];
            }
                
        }
        return null;
    }

    public SlotMap getSpawnPoint(int index)
    {
        return spawnPoints[index];
    }

    public int SpawnPointCount => spawnPoints.Count;

    public void gameUpdate()
    {
        for (int i = 0; i < updatingContent.Count; i++)
        {
            updatingContent[i].gameUpdate();
        }
    }

    public void ToggleDestination(SlotMap slot) 
    {
        if (slot.Content.Type == GameSlotContentType.Destination) 
        {
            slot.Content = contentFactory.get(GameSlotContentType.Empty);
        }
    }

    public void ToggleWall (SlotMap slot) 
    {
        if (slot.Content.Type == GameSlotContentType.Wall)
        {
            slot.Content = contentFactory.get(GameSlotContentType.Empty);
        }
        else if (slot.Content.Type == GameSlotContentType.Empty)
        {
            slot.Content = contentFactory.get(GameSlotContentType.Wall);
        }
    }

    public void ToggleTower(SlotMap slot)
    {
        if (slot.Content.Type == GameSlotContentType.Tower)
        {
            updatingContent.Remove(slot.Content);
            slot.Content = contentFactory.get(GameSlotContentType.Empty);
        }
        else if (slot.Content.Type == GameSlotContentType.Empty)
        {
            slot.Content = contentFactory.get(GameSlotContentType.Tower);
            updatingContent.Add(slot.Content);
        }
    }

    public void ToggleSpawnPoint(SlotMap slot)
    {
        if (slot.Content.Type == GameSlotContentType.SpawnPoint)
        {           
            if(spawnPoints.Count > 1) 
            {
                spawnPoints.Remove(slot);
                slot.Content = contentFactory.get(GameSlotContentType.Empty);
            }
        }
        else if (slot.Content.Type == GameSlotContentType.Empty)
        {
            slot.Content = contentFactory.get(GameSlotContentType.SpawnPoint);
            spawnPoints.Add(slot);
        }
    }


}
