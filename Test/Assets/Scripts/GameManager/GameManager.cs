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

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    void Awake()
    {
        map.Initialize(mapSize);
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
            HandleTouch();
        }

        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }
    }

    void HandleTouch()
    {
        SlotMap slot = map.getSlot(TouchRay);
        if (slot != null)
        {
            slot.Content =
            tileContentFactory.get(GameSlotContentType.Destination);
        }
    }

    void HandleAlternativeTouch()
    {
        SlotMap slot = map.getSlot(TouchRay);
        if (slot != null)
        {
            slot.Content =
            tileContentFactory.get(GameSlotContentType.Tower);
        }
    }
}
