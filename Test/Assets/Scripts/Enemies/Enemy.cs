using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;
    [SerializeField]
    float speedEnemy = 5f;

    public EnemyFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    public void spawnOn(SlotMap slot)
    {
        transform.localPosition = slot.transform.localPosition;
    }

    public bool gameUpdate()
    {
        transform.localPosition += Vector3.forward * Time.deltaTime * speedEnemy;
        return true;
    }
}
