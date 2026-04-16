using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;
    float speedEnemy;

    [SerializeField]
    HealthBar healthBar;
    public EnemyFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    bool iTouchTheBase;

    public bool IsTouchTheBase => iTouchTheBase;

    float health {  get; set; }

    float damage;

    public float getDamage => damage;

    SlotMap slotFrom, slotTo;
    Vector3 positionFrom, positionTo;
    float progress;

    Direction direction;
    DirectionChange directionChange;
    float directionAngleFrom, directionAngleTo;

    public void spawnOn(SlotMap slot)
    {
        Debug.Assert(slot.NextTileOnPath != null, "Nowhere to go!", this);
        slotFrom = slot;
        slotTo = slot.NextTileOnPath;
        
        progress = 0f;
        PrepareIntro();
    }

    void PrepareIntro()
    {
        positionFrom = slotFrom.transform.localPosition;
        positionTo = slotFrom.ExitPoint;
        direction = slotFrom.PathDirection;
        directionChange = DirectionChange.None;
        directionAngleFrom = directionAngleTo = direction.GetAngle();
        transform.localRotation = direction.GetRotation();
    }

    public bool gameUpdate()
    {
        progress += Time.deltaTime;
        while (progress >= 1f)
        {
            slotFrom = slotTo;
            slotTo = slotTo.NextTileOnPath;
            if (slotTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }

            
            progress -= 1f;
            PrepareNextState();
        }
        transform.localPosition =
            Vector3.LerpUnclamped(positionFrom, positionTo, progress);

        if (directionChange != DirectionChange.None)
        {
            float angle = Mathf.LerpUnclamped(
                directionAngleFrom, directionAngleTo, progress
            );
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
        return true;
    }

    void PrepareNextState()
    {
        positionFrom = positionTo;
        positionTo = slotFrom.ExitPoint;
        directionChange = direction.GetDirectionChangeTo(slotFrom.PathDirection);
        direction = slotFrom.PathDirection;
        directionAngleFrom = directionAngleTo;

        switch (directionChange)
        {
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
            default: PrepareTurnAround(); break;
        }
    }

    void PrepareForward()
    {
        transform.localRotation = direction.GetRotation();
        directionAngleTo = direction.GetAngle();
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom + 90f;
    }

    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom - 90f;
    }

    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + 180f;
    }

    public void initialize(float speed, float initialHealth, float damage)
    {
        speedEnemy = speed;
        health = initialHealth;
        healthBar.initHealthBar(initialHealth);
        iTouchTheBase = false;
        this.damage = damage;
    }

    public void applyDamage(float damage)
    {
        Debug.Assert(damage >= 0f, "Negative damage applied.");
        health -= damage;
        healthBar.updateHealthBar(health);
    }

    public void deathForTouchBase() 
    {
        iTouchTheBase = true;
        health = 0;
    }
}
