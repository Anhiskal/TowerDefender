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

    public void spawnOn(SlotMap slot)
    {
        transform.localPosition = slot.transform.localPosition;
    }

    public bool gameUpdate()
    {
        if (health <= 0f) 
        {
            originFactory.Reclaim(this);
            return false;
        }
        transform.localPosition += Vector3.forward * Time.deltaTime * speedEnemy;
        return true;
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
