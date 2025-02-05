using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SlotMap;

public class GameSlotContent : MonoBehaviour
{
    [SerializeField]
    GameSlotContentType type = default;

    public GameSlotContentType Type => type;

    GameSlotContentFactory originFactory;

    TargetPoint target;

    public GameSlotContentFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    public void Recycle()
    {
        originFactory.reclaim(this);
    }

    public virtual void gameUpdate()
    {
        
    }

}
