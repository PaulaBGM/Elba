using UnityEngine;

public class ShelterSleepSystem : MonoBehaviour
{
    [SerializeField] private PlayerStatsSystem playerStats;

    public void Sleep()
    {
        playerStats.SetStat(
            StatType.Health,
            playerStats.GetMax(StatType.Health));

        playerStats.SetStat(
            StatType.Stamina,
            playerStats.GetMax(StatType.Stamina));

        Debug.Log("Sleeping...");
    }
}