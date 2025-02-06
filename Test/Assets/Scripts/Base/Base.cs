using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField]
    HealthBar healthBar;

    float health { get; set; }

    private void Awake()
    {
        initebase(3);
    }

    void initebase(float healtMax) 
    {
        health = healtMax;
        healthBar.initHealthBar(health);
    }

    public void applyDamage(float damage)
    {
        Debug.Assert(damage >= 0f, "Negative damage applied.");
        health -= damage;
        healthBar.updateHealthBar(health);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null) 
        {
            Debug.Log("se detecto un enemigo");
            applyDamage(enemy.getDamage);
            enemy.deathForTouchBase();
        }
    }
}
