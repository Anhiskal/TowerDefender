using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static SlotMap;

public class GameMap : MonoBehaviour
{
    [SerializeField]
    Transform map = default;

    [SerializeField]
    SlotMap slotPrefab = default;

    [SerializeField]
    Texture2D gridTexture = default;

    Vector2Int size;

    SlotMap[] slots;

    GameSlotContentFactory contentFactory;

    bool showGrid, showPaths;

    List<SlotMap> spawnPoints = new List<SlotMap>();
    List<GameSlotContent> updatingContent = new List<GameSlotContent>();
    Queue<SlotMap> searchFrontier = new Queue<SlotMap>();

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

                if (x > 0)
                {
                    SlotMap.MakeEastWestNeighbors(slot, slots[i - 1]);
                }
                if (y > 0)
                {
                    SlotMap.MakeNorthSouthNeighbors(slot, slots[i - size.x]);
                }

                slot.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    slot.IsAlternative = !slot.IsAlternative;
                }

                slot.Content = contentFactory.get(GameSlotContentType.Empty);
            }
        }

        ToggleDestination(slots[slots.Length / 2]);
    }

    bool FindPaths()
    {
        foreach (SlotMap tile in slots)
        {
            if (tile.Content.Type == GameSlotContentType.Destination)
            {
                tile.BecomeDestination();
                searchFrontier.Enqueue(tile);
            }
            else
            {
                tile.ClearPath();
            }            
        }

        if (searchFrontier.Count == 0)
        {
            return false;
        }

        while (searchFrontier.Count > 0)
        {
            
            SlotMap tile = searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    searchFrontier.Enqueue(tile.GrowPathNorth());
                    searchFrontier.Enqueue(tile.GrowPathSouth());
                    searchFrontier.Enqueue(tile.GrowPathEast());                
                    searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    searchFrontier.Enqueue(tile.GrowPathWest());
                    searchFrontier.Enqueue(tile.GrowPathEast());
                    searchFrontier.Enqueue(tile.GrowPathSouth());
                    searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }
            
        }

        foreach (SlotMap tile in slots)
        {
            if (!tile.HasPath)
            {
                return false;
            }
        }

        if (showPaths) 
        {
            foreach (SlotMap tile in slots)
            {
                tile.ShowPath();
            }
        }            

        return true;
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

    public void ToggleEmpty(SlotMap slot) 
    {
        switch (slot.Content.Type) 
        {
            case GameSlotContentType.Destination: break;
            case GameSlotContentType.Wall: break;
            case GameSlotContentType.Tower:
                updatingContent.Remove(slot.Content); 
                break;
            case GameSlotContentType.SpawnPoint:
                spawnPoints.Remove(slot); 
                break;
        }

        slot.Content = contentFactory.get(GameSlotContentType.Empty);
    }

    public void ToggleDestination(SlotMap slot)
    {
        if (slot.Content.Type == GameSlotContentType.Destination)
        {
            slot.Content = contentFactory.get(GameSlotContentType.Empty);
            if (!FindPaths())
            {
                slot.Content =
                    contentFactory.get(GameSlotContentType.Destination);
                FindPaths();
            }
        }
        else if (slot.Content.Type == GameSlotContentType.Empty)
        {
            slot.Content = contentFactory.get(GameSlotContentType.Destination);
            FindPaths();
        }

    }

    public void ToggleWall (SlotMap slot) 
    {
        if (slot.Content.Type == GameSlotContentType.Wall)
        {
            slot.Content = contentFactory.get(GameSlotContentType.Empty);
            FindPaths();
        }
        else if(slot.Content.Type == GameSlotContentType.Empty)
        {
            slot.Content = contentFactory.get(GameSlotContentType.Wall);
            if (!FindPaths())
            {
                slot.Content = contentFactory.get(GameSlotContentType.Empty);
                FindPaths();
            }
        }
    }

    public void ToggleTower(SlotMap slot)
    {
        /*if (slot.Content.Type == GameSlotContentType.Tower)
        {
            updatingContent.Remove(slot.Content);
            slot.Content = contentFactory.get(GameSlotContentType.Empty);
        }
        else */if (slot.Content.Type == GameSlotContentType.Empty)
        {
            slot.Content = contentFactory.get(GameSlotContentType.Tower);
            updatingContent.Add(slot.Content);
        }
    }

    public void ToggleSpawnPoint(SlotMap slot)
    {
        /*if (slot.Content.Type == GameSlotContentType.SpawnPoint)
        {           
            if(spawnPoints.Count > 1) 
            {
                spawnPoints.Remove(slot);
                slot.Content = contentFactory.get(GameSlotContentType.Empty);
            }
        }
        else */if (slot.Content.Type == GameSlotContentType.Empty)
        {
            slot.Content = contentFactory.get(GameSlotContentType.SpawnPoint);
            spawnPoints.Add(slot);
        }
    }

    public bool ShowPaths
    {
        get => showPaths;
        set
        {
            showPaths = value;
            if (showPaths)
            {
                foreach (SlotMap tile in slots)
                {
                    tile.ShowPath();
                }
            }
            else
            {
                foreach (SlotMap tile in slots)
                {
                    tile.HidePath();
                }
            }
        }
    }

    public bool ShowGrid
    {
        get => showGrid;
        set
        {
            showGrid = value;
            Material m = map.GetComponent<MeshRenderer>().material;
            if (showGrid)
            {
                m.mainTexture = gridTexture;
                m.SetTextureScale("_MainTex", size);
            }
            else
            {
                m.mainTexture = null;
            }
        }
    }


}
