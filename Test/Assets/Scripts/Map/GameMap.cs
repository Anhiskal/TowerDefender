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

    public void Initialize(Vector2Int size)
    {
        this.size = size;
        map.localScale = new Vector3(size.x, size.y, 1f);

       

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
            }
        }

    }

    public SlotMap getSlot(Ray ray) 
    {
        if (Physics.Raycast(ray, out RaycastHit hit)) 
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
    }

    public void ToggleTower(SlotMap slot) { }


}
