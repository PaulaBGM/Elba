using UnityEngine;

public class ShelterData : MonoBehaviour
{
    [SerializeField] private int level = 1;

    public int Level => level;

    public void Upgrade()
    {
        level++;
    }

    public float GetColdProtection()
    {
        return level switch
        {
            1 => 0.25f,
            2 => 0.50f,
            3 => 0.75f,
            4 => 1f,
            _ => 1f
        };
    }
}