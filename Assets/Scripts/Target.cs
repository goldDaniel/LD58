using UnityEngine;
public class Target : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 100f)]
    private float maxHealth;
    public float CurrentHealth { get; private set; }

    public float HealthPercentage => CurrentHealth / maxHealth;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }
}