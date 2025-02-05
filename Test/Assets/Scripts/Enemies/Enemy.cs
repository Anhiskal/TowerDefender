using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;
    float speedEnemy;    

    public EnemyFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }
   

    float health {  get; set; }

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

    public void initialize(float speed, float initialHealth)
    {
        speedEnemy = speed;
        health = initialHealth;
    }

    public void applyDamage(float damage)
    {
        Debug.Assert(damage >= 0f, "Negative damage applied.");
        health -= damage;
    }
}
