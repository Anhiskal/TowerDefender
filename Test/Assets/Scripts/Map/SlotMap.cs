using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMap : MonoBehaviour
{
    [SerializeField]
    Transform indication = default;

    SlotMap north, east, south, west;    

    public enum GameSlotContentType
    {
        Empty, Destination, Wall, Tower, SpawnPoint
    }

    GameSlotContent content;

    public GameSlotContent Content
    {
        get => content;
        set
        {
            Debug.Assert(value != null, "Null assigned to content!");
            if (content != null)
            {
                content.Recycle();
            }
            content = value;
            content.transform.localPosition = transform.localPosition;
        }
    }
}


