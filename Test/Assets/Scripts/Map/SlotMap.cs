using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMap : MonoBehaviour
{
    [SerializeField]
    Transform arrow = default;

    SlotMap north, east, south, west, nextOnPath;
    int distance;

    public enum GameSlotContentType
    {
        Empty, Destination, Wall, Tower, SpawnPoint
    }

    GameSlotContent content;

    public bool HasPath => distance != int.MaxValue;

    public SlotMap GrowPathNorth() => GrowPathTo(north);

    public SlotMap GrowPathEast() => GrowPathTo(east);

    public SlotMap GrowPathSouth() => GrowPathTo(south);

    public SlotMap GrowPathWest() => GrowPathTo(west);

    public bool IsAlternative { get; set; }

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

    public static void MakeEastWestNeighbors(SlotMap east, SlotMap west)
    {
        Debug.Assert(
            west.east == null && east.west == null, "Redefined neighbors!"
        );
        west.east = east;
        east.west = west;
    }

    public static void MakeNorthSouthNeighbors(SlotMap north, SlotMap south)
    {
        Debug.Assert(
            south.north == null && north.south == null, "Redefined neighbors!"
        );
        south.north = north;
        north.south = south;
    }

    public void ClearPath()
    {
        distance = int.MaxValue;
        nextOnPath = null;
    }
    public void BecomeDestination()
    {
        distance = 0;
        nextOnPath = null;
    }

    SlotMap GrowPathTo(SlotMap neighbor)
    {
        Debug.Assert(HasPath, "No path!");

        if (neighbor == null || neighbor.HasPath)
        {
            return null;
        }
        neighbor.distance = distance + 1;
        neighbor.nextOnPath = this;

        return neighbor.Content.Type != GameSlotContentType.Wall ? neighbor : null;
    }

    static Quaternion
        northRotation = Quaternion.Euler(90f, 0f, 0f),
        eastRotation = Quaternion.Euler(90f, 90f, 0f),
        southRotation = Quaternion.Euler(90f, 180f, 0f),
        westRotation = Quaternion.Euler(90f, 270f, 0f);

    public void ShowPath()
    {
        if (distance == 0)
        {
            arrow.gameObject.SetActive(false);
            return;
        }
        arrow.gameObject.SetActive(true);
        arrow.localRotation =
            nextOnPath == north ? northRotation :
            nextOnPath == east ? eastRotation :
            nextOnPath == south ? southRotation :
            westRotation;
    }

    public void HidePath()
    {
        arrow.gameObject.SetActive(false);
    }
}


