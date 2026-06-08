using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerHealthSystem health = character.GetComponent<PlayerHealthSystem>();
        if (health != null)
            health.AddHealth((int)val);
    }
}