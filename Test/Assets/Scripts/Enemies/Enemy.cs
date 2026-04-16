using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;
    float speedEnemy;

    [SerializeField]
    Transform model = default;

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

    float pathOffset;

    public bool IsTouchTheBase => iTouchTheBase;

    float health {  get; set; }

    float damage;

    public float getDamage => damage;

    SlotMap slotFrom, slotTo;
    Vector3 positionFrom, positionTo;
    float progress, progressFactor;

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

    public bool gameUpdate()
    {
        progress += Time.deltaTime * progressFactor;
        while (progress >= 1f)
        {            
            if (slotTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }

            progress = (progress - 1f) / progressFactor;
            PrepareNextState();
            progress *= progressFactor;
        }
        if (directionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        }
        else
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
        slotFrom = slotTo;
        slotTo = slotTo.NextTileOnPath;
        positionFrom = positionTo;
        if (slotTo == null)
        {
            PrepareOutro();
            return;
        }
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

    void PrepareIntro()
    {
        positionFrom = slotFrom.transform.localPosition;
        positionTo = slotFrom.ExitPoint;
        direction = slotFrom.PathDirection;
        directionChange = DirectionChange.None;
        directionAngleFrom = directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speedEnemy;
    }

    void PrepareForward()
    {
        transform.localRotation = direction.GetRotation();
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        progressFactor = speedEnemy;
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(pathOffset - 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speedEnemy / (Mathf.PI * 0.5f * (0.5f - pathOffset));
    }

    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(pathOffset + 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speedEnemy / (Mathf.PI * 0.5f * (0.5f + pathOffset));
    }

    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + (pathOffset < 0f ? 180f : -180f);
        model.localPosition = new Vector3(pathOffset, 0f);
        transform.localPosition = positionFrom;
        progressFactor = speedEnemy / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffset), 0.2f));
    }

    void PrepareOutro()
    {
        positionTo = slotFrom.transform.localPosition;
        directionChange = DirectionChange.None;
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speedEnemy;
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

    public void Initialize(float scale, float pathOffset)
    {
        model.localScale = new Vector3(scale, scale, scale);
        this.pathOffset = pathOffset;
    }
}
