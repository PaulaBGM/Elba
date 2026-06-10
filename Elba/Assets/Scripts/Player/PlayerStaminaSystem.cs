using UnityEngine;

public class PlayerStaminaSystem : MonoBehaviour
{
    [SerializeField] private PlayerStatsSystem stats;

    [SerializeField] private float staminaDrainPerSecond = 15f;
    [SerializeField] private float staminaRegenPerSecond = 10f;

    private bool isSprinting;

    private void Awake()
    {
        if (stats == null)
            stats = GetComponent<PlayerStatsSystem>();
    }

    private void Update()
    {
        if (isSprinting)
        {
            stats.ModifyStat(StatType.Stamina, -staminaDrainPerSecond * Time.deltaTime);
        }
        else
        {
            stats.ModifyStat(StatType.Stamina,staminaRegenPerSecond * Time.deltaTime);
        }
    }

    public void SetSprinting(bool value)
    {
        isSprinting = value;
    }

    public bool HasStamina()
    {
        return !stats.IsEmpty(StatType.Stamina);
    }
    public bool CanSprint()
    {
        return !stats.IsEmpty(StatType.Stamina);
    }
}