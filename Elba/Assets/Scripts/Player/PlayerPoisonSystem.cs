using System.Collections;
using UnityEngine;

public class PlayerPoisonSystem : MonoBehaviour
{
    private Coroutine poisonRoutine;

    public bool IsPoisoned => poisonRoutine != null;

    public void ApplyPoison(float totalDamage, float duration)
    {
        Debug.Log( $"Aplicando veneno | Daño: {totalDamage} | Duración: {duration}");

        if (poisonRoutine != null)
            StopCoroutine(poisonRoutine);

        poisonRoutine =  StartCoroutine(PoisonRoutine(totalDamage,duration));
    }

    public void CurePoison()
    {
        if (poisonRoutine != null)
        {
            StopCoroutine(poisonRoutine);
            poisonRoutine = null;
        }

        Debug.Log("Veneno curado");
    }

    private IEnumerator PoisonRoutine(float totalDamage,float duration)
    {
        PlayerStatsSystem stats = GetComponent<PlayerStatsSystem>();

        if (stats == null)
            yield break;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float damagePerSecond = totalDamage / duration;

            stats.ModifyStat(StatType.Health, -damagePerSecond);

            elapsed += 1f;

            yield return new WaitForSeconds(1f);
        }

        poisonRoutine = null;
    }
}