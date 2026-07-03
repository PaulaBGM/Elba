using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Stat Modifier")]
public class CharacterStatModifierSO : ScriptableObject
{
    [SerializeField] private StatType statType;

    [Header("UI")]
    [SerializeField] private Sprite icon;

    public StatType StatType => statType;
    public Sprite Icon => icon;

    public void AffectCharacter(GameObject character, float value)
    {
        PlayerStatsSystem stats = character.GetComponent<PlayerStatsSystem>();

        if (stats == null)
            return;

        stats.ModifyStat(statType, value);
    }
}