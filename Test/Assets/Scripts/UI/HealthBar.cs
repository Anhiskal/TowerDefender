using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Slider healthBar;

    float healthMax;
    float percentageCurrentHealth;

    public void initHealthBar(float healthMax) 
    {
        healthBar.value = 1;
        this.healthMax = healthMax;
    }

    public void updateHealthBar(float currentHealth) 
    {
        percentageCurrentHealth = currentHealth / healthMax;
        healthBar.value = percentageCurrentHealth;
    }
}
