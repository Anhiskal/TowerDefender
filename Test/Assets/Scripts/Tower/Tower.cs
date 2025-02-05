using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower : GameSlotContent
{
    const int enemyLayerMask = 1 << 9;

    [SerializeField]
    Transform laserPoint = default,  pointGenerator = default;
    Vector3 lasertScale;

    [SerializeField, Range(1.5f, 10.5f)]
    float targetingRange = 1.5f;

    [SerializeField, Range(1f, 100f)]
    float damagePerSecond = 10f;

    TargetPoint target;

    static Collider[] targetsBuffer = new Collider[1];


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, targetingRange);
        if (target != null)
        {
            Gizmos.DrawLine(position, target.position);
        }
    }

    private void Awake()
    {
        lasertScale = Vector3.one * 0.1f;
    }

    public override void gameUpdate()
    {
        if (trackTarget() || AcquireTarget())
        {
            Debug.Log("Acquired target!");
            shoot();
        }
        else
        {
            laserPoint.localScale = Vector3.zero;
        }
    }

    bool AcquireTarget()
    {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 3f;

        int hits = Physics.OverlapCapsuleNonAlloc(
            a, b, targetingRange, targetsBuffer, enemyLayerMask
        );
        if (hits > 0)
        {
            target = targetsBuffer[0].GetComponent<TargetPoint>();
            Debug.Assert(target != null, "Targeted non-enemy!", targetsBuffer[0]);
            return true;
        }
        target = null;
        return false;
    }

    bool trackTarget()
    {
        if (target == null)
        {
            return false;
        }

        Vector3 myPosition = transform.localPosition;
        Vector3 positionOfEnemy = target.position;

        if (Vector3.Distance(myPosition, positionOfEnemy) > targetingRange) 
        {
            target = null;
            return false;
        }
        return true;
    }

    void shoot() 
    {
        Vector3 point = target.position;
        pointGenerator.LookAt(point);
        laserPoint.localRotation = pointGenerator.localRotation;

        float d = Vector3.Distance(pointGenerator.position, point);
        lasertScale.z = d;
        laserPoint.localScale = lasertScale;
        laserPoint.localPosition = pointGenerator.localPosition + 0.5f * d * laserPoint.forward;

        target.enemy.applyDamage(damagePerSecond * Time.deltaTime);
    }
}
